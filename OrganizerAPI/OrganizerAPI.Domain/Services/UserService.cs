using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OrganizerAPI.Domain.Interfaces;
using OrganizerAPI.Infrastructure.Contexts;
using OrganizerAPI.Infrastructure.Repositories;
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
using System.Threading.Tasks;

namespace OrganizerAPI.Domain.Services
{
    public class UserService : IUserService
    {
        /// <summary>
        /// JWT token expiration time in seconds.
        /// </summary>
        private const int JWT_TOKEN_EXPIRES_IN = 1800;

        private OrganizerContext context;
        private readonly UserRepository userRepository;
        private readonly JWTSettings jwtSettings;
        private readonly IMapper mapper;
        private readonly AbstractValidator<UserRequestDTO> validator;

        public UserService(
            OrganizerContext context,
            UserRepository userRepository,
            IOptions<JWTSettings> jwtSettings,
            IMapper mapper,
            AbstractValidator<UserRequestDTO> validator)
        {
            this.context = context;
            this.userRepository = userRepository;
            this.jwtSettings = jwtSettings.Value;
            this.mapper = mapper;
            this.validator = validator;
        }

        public async Task<List<UserDTO>> GetAll(int? userId = null)
        {
            try
            {
                var result = new List<UserDTO>();
                foreach (var item in await userRepository.GetList())
                {
                    result.Add(mapper.MapUser(item));
                }
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<UserDTO> GetById(int id, int? userId = null)
        {
            try
            {
                return mapper.MapUser(await userRepository.GetById(id));
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<UserDTO> Create(UserDTO entity, int? userId = null)
        {
            try
            {
                var validationResult = validator.Validate(entity as UserRequestDTO);
                if (!validationResult.IsValid)
                    throw new ValidationException(validationResult.Errors);
                return mapper.MapUser(await userRepository.Create(mapper.MapUser(entity as UserRequestDTO)));                  
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<UserDTO> Update(UserDTO entity, int? userId = null)
        {
            try
            {
                if (entity.Id == userId || (await userRepository.GetById(userId.Value)).Role.Name == "Admin")
                {
                    var validationResult = validator.Validate(entity as UserRequestDTO);
                    if (!validationResult.IsValid)
                        throw new ValidationException(validationResult.Errors);
                    return mapper.MapUser(await userRepository.Update(mapper.MapUser(entity as UserRequestDTO)));
                }
                throw new Exception("User doesn't have sufficient rights to receive the data.");
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task Delete(UserDTO entity, int? userId = null)
        {
            if (entity.Id == userId || (await userRepository.GetById(userId.Value)).Role.Name == "Admin")
            {
                await userRepository.Delete(mapper.MapUser(entity));
            }
            throw new Exception("User doesn't have sufficient rights to receive the data.");
        }

        public async Task DeleteById(int id, int? userId = null)
        {
            if (id == userId || (await userRepository.GetById(userId.Value)).Role.Name == "Admin")
            {
                await userRepository.DeleteById(id);
            }
            throw new Exception("User doesn't have sufficient rights to receive the data.");
        }

        public async Task<UserAuthResponseDTO> Registration(UserRequestDTO model, string ipAddress)
        {
            try
            {
                var user = mapper.MapUser(await Create(model));

                var tokens = generateAndSaveTokensPair(user, ipAddress);

                return new UserAuthResponseDTO(mapper.MapUser(user), tokens.JwtToken, JWT_TOKEN_EXPIRES_IN, tokens.RefreshToken.Token);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public UserAuthResponseDTO Authenticate(UserAuthRequestDTO model, string ipAddress)
        {
            var user = context.Users.SingleOrDefault(x => x.Username == model.Username && x.Password == model.Password);

            // return null if user not found
            if (user == null) 
                return null;

            var tokens = generateAndSaveTokensPair(user, ipAddress);

            return new UserAuthResponseDTO(mapper.MapUser(user), tokens.JwtToken, JWT_TOKEN_EXPIRES_IN, tokens.RefreshToken.Token);
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

            var tokens = generateAndSaveTokensPair(user, ipAddress);

            return new UserAuthResponseDTO(mapper.MapUser(user), tokens.JwtToken, JWT_TOKEN_EXPIRES_IN, tokens.RefreshToken.Token);
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

        public UserDTO GetCurrentUser(string token)
        {
            var user = context.Users.Include(u => u.UserRefreshTokens).SingleOrDefault(u => u.UserRefreshTokens.Any(rt => rt.Token == token));

            return mapper.MapUser(user);
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
                Expires = DateTime.UtcNow.AddMinutes(JWT_TOKEN_EXPIRES_IN),
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

        private TokensPair generateAndSaveTokensPair(User user, string ipAddress)
        {
            // generate jwt and refresh tokens
            var jwtToken = generateJwtToken(user);
            var refreshToken = generateRefreshToken(ipAddress);

            // revoke last used refresh token
            var refreshTokens = user.UserRefreshTokens;
            if (refreshTokens != null && refreshTokens.Count != 0)
            {
                var oldJwtToken = refreshTokens.Last();
                oldJwtToken.Revoked = DateTime.UtcNow;
                oldJwtToken.RevokedByIp = ipAddress;
                oldJwtToken.ReplacedByToken = refreshToken.Token;
            }

            // save refresh token
            user.UserRefreshTokens.Add(refreshToken);
            context.Update(user);
            context.SaveChanges();

            return new TokensPair(jwtToken, refreshToken);
        }
    }
}
