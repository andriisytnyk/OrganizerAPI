using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OrganizerAPI.Domain.Interfaces;
using OrganizerAPI.Infrastructure.Contexts;
using OrganizerAPI.Models.Common;
using OrganizerAPI.Models.Models;
using OrganizerAPI.Shared;
using OrganizerAPI.Shared.ModelsDTO;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace OrganizerAPI.Domain.Services
{
    public class UserService : IUserService
    {
        private OrganizerContext context;
        private readonly JWTSettings jwtSettings;
        private readonly IMapper mapper;
        private readonly AbstractValidator<UserDTO> validator;

        public UserService(
            OrganizerContext context,
            IOptions<JWTSettings> jwtSettings,
            IMapper mapper)
        {
            this.context = context;
            this.jwtSettings = jwtSettings.Value;
            this.mapper = mapper;
        }

        public UserAuthResponseDTO Authenticate(UserAuthRequestDTO model, string ipAddress)
        {
            var user = context.Users.SingleOrDefault(x => x.Username == model.Username && x.Password == model.Password);

            // return null if user not found
            if (user == null) 
                return null;

            // authentication successful so generate jwt and refresh tokens
            var jwtToken = generateJwtToken(user);
            var refreshToken = generateRefreshToken(ipAddress);

            // save refresh token
            user.UserRefreshTokens.Add(refreshToken);
            context.Update(user);
            context.SaveChanges();

            return new UserAuthResponseDTO(mapper.MapUser(user), jwtToken, refreshToken.Token);
        }

        public UserAuthResponseDTO UpdateRefreshToken(string token, string ipAddress)
        {
            var user = context.Users.Include(u => u.UserRefreshTokens).SingleOrDefault(u => u.UserRefreshTokens.Any(rt => rt.Token == token));

            // return null if no user found with token
            if (user == null) 
                return null;

            var refreshToken = user.UserRefreshTokens.Single(rt => rt.Token == token);

            // return null if token is no longer active
            if (!refreshToken.IsActive) 
                return null;

            // replace old refresh token with a new one and save
            var newRefreshToken = generateRefreshToken(ipAddress);
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            refreshToken.ReplacedByToken = newRefreshToken.Token;
            user.UserRefreshTokens.Add(newRefreshToken);
            context.Update(user);
            context.SaveChanges();

            // generate new jwt
            var jwtToken = generateJwtToken(user);

            return new UserAuthResponseDTO(mapper.MapUser(user), jwtToken, newRefreshToken.Token);
        }

        public bool RevokeToken(string token, string ipAddress)
        {
            var user = context.Users.SingleOrDefault(u => u.UserRefreshTokens.Any(rt => rt.Token == token));

            // return false if no user found with token
            if (user == null) 
                return false;

            var refreshToken = user.UserRefreshTokens.Single(rt => rt.Token == token);

            // return false if token is not active
            if (!refreshToken.IsActive) 
                return false;

            // revoke token and save
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            context.Update(user);
            context.SaveChanges();

            return true;
        }

        public IEnumerable<User> GetAll()
        {
            return context.Users.Include(u => u.UserRefreshTokens);
        }

        public User GetById(int id)
        {
            return context.Users.Include(u => u.UserRefreshTokens).SingleOrDefault(u => u.Id == id);
        }

        public int? GetCurrentUserId(string token)
        {
            var user = context.Users.Include(u => u.UserRefreshTokens).SingleOrDefault(u => u.UserRefreshTokens.Any(rt => rt.Token == token));

            return user?.Id;
        }

        private string generateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private RefreshToken generateRefreshToken(string ipAddress)
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);
                return new RefreshToken
                {
                    Token = Convert.ToBase64String(randomBytes),
                    Expires = DateTime.UtcNow.AddDays(7),
                    Created = DateTime.UtcNow,
                    CreatedByIp = ipAddress
                };
            }
        }
    }
}
