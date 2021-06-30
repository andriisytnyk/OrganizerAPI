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
using OrganizerAPI.Shared.Exceptions;

namespace OrganizerAPI.Domain.Services
{
    public class UserService : IUserService
    {
        /// <summary>
        /// JWT token expiration time in seconds.
        /// </summary>
        private const int JwtTokenExpiresIn = 1800;

        private readonly OrganizerContext _context;
        private readonly UserRepository _userRepository;
        private readonly JwtSettings _jwtSettings;
        private readonly IMapper _mapper;
        private readonly AbstractValidator<UserDto> _userDtoValidator;
        private readonly AbstractValidator<UserRequestDto> _userRequestDtoValidator;

        public UserService(
            OrganizerContext context,
            UserRepository userRepository,
            IOptions<JwtSettings> jwtSettings,
            IMapper mapper,
            AbstractValidator<UserDto> userDtoValidator,
            AbstractValidator<UserRequestDto> userRequestDtoValidator)
        {
            _context = context;
            _userRepository = userRepository;
            _jwtSettings = jwtSettings.Value;
            _mapper = mapper;
            _userDtoValidator = userDtoValidator;
            _userRequestDtoValidator = userRequestDtoValidator;
        }

        public async Task<List<UserDto>> GetAll(int? userId = null)
        {
            try
            {
                return (await _userRepository.GetList()).Select(u => _mapper.MapUser(u)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserDto> GetById(int id, int? userId = null)
        {
            try
            {
                var user = await _userRepository.GetById(id);
                if (user == null)
                    throw new UserNotFoundException("User with such id was not found.");
                return _mapper.MapUser(user);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserDto> Create(UserDto entity, int? userId = null)
        {
            try
            {
                if (userId == null)
                    throw new Exception("UserId was not included.");
                if ((await _userRepository.GetById(userId.Value)).Role != Role.Admin)
                    throw new Exception("User doesn't have sufficient rights to create new user.");

                var validationResult = await _userRequestDtoValidator.ValidateAsync(entity as UserRequestDto);
                if (!validationResult.IsValid)
                    throw new ValidationException(validationResult.Errors);

                return _mapper.MapUser(await _userRepository.Create(_mapper.MapUser(entity as UserRequestDto)));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserDto> Update(UserDto entity, int? userId = null)
        {
            try
            {
                if (userId == null)
                    throw new Exception("UserId was not included.");
                if (entity.Id != userId && (await _userRepository.GetById(userId.Value)).Role != Role.Admin)
                    throw new Exception("User doesn't have sufficient rights to receive the data.");

                var validationResult = await _userDtoValidator.ValidateAsync(entity);
                if (!validationResult.IsValid)
                    throw new ValidationException(validationResult.Errors);

                return _mapper.MapUser(await _userRepository.Update(_mapper.MapUser(entity as UserRequestDto)));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Delete(UserDto entity, int? userId = null)
        {
            if (userId == null)
                throw new Exception("UserId was not included.");
            if (entity.Id != userId && (await _userRepository.GetById(userId.Value)).Role != Role.Admin)
                throw new Exception("User doesn't have sufficient rights to receive the data.");

            await _userRepository.Delete(_mapper.MapUser(entity));
        }

        public async Task DeleteById(int id, int? userId = null)
        {
            if (userId == null)
                throw new Exception("UserId was not included.");
            if (id != userId && (await _userRepository.GetById(userId.Value)).Role != Role.Admin)
                throw new Exception("User doesn't have sufficient rights to receive the data.");

            await _userRepository.DeleteById(id);
        }

        public async Task<UserAuthResponseDto> Registration(UserRequestDto model, string ipAddress)
        {
            try
            {
                var validationResult = await _userRequestDtoValidator.ValidateAsync(model);
                if (!validationResult.IsValid)
                    throw new ValidationException(validationResult.Errors);

                if ((await _userRepository.GetList()).Any(u => u.Username == model.Username))
                    throw new Exception("User with the same username already exists.");
                
                var user = await _userRepository.Create(_mapper.MapUser(model));

                var tokens = GenerateAndSaveTokensPair(user, ipAddress);

                return new UserAuthResponseDto(_mapper.MapUser(user), tokens.JwtToken, JwtTokenExpiresIn, tokens.RefreshToken.Token);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public UserAuthResponseDto Authenticate(UserAuthRequestDto model, string ipAddress)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Password))
                    throw new InvalidAuthDataException("Username and password are required.");

                var user = _context.Users.SingleOrDefault(x => x.Username == model.Username && x.Password == model.Password);

                if (user == null)
                    throw new InvalidAuthDataException("Username or password is incorrect.");

                var tokens = GenerateAndSaveTokensPair(user, ipAddress);

                return new UserAuthResponseDto(_mapper.MapUser(user), tokens.JwtToken, JwtTokenExpiresIn, tokens.RefreshToken.Token);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public UserAuthResponseDto UpdateRefreshToken(string token, string ipAddress)
        {
            try
            {
                var user = _context.Users.Include(u => u.UserRefreshTokens).SingleOrDefault(u => u.UserRefreshTokens.Any(rt => rt.Token == token));

                // return exception if no user found with token
                if (user == null)
                    throw new UserNotFoundException("User with such token was not found.");

                var refreshToken = user.UserRefreshTokens.Single(rt => rt.Token == token);

                // return exception if token is no longer active
                if (!refreshToken.IsActive)
                    throw new NotActiveTokenException("Token is not longer active.");

                var tokens = GenerateAndSaveTokensPair(user, ipAddress);

                return new UserAuthResponseDto(_mapper.MapUser(user), tokens.JwtToken, JwtTokenExpiresIn, tokens.RefreshToken.Token);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void RevokeToken(string token, string ipAddress)
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                    throw new Exception("Token is required");

                var user = _context.Users.Include(u => u.UserRefreshTokens).SingleOrDefault(u => u.UserRefreshTokens.Any(rt => rt.Token == token));

                // return exception if no user found with token
                if (user == null)
                    throw new UserNotFoundException("User with such token was not found.");

                var refreshToken = user.UserRefreshTokens.Single(rt => rt.Token == token);

                // return exception if token is not active
                if (!refreshToken.IsActive)
                    throw new NotActiveTokenException("Token is not longer active.");

                // revoke token and save
                refreshToken.Revoked = DateTime.UtcNow;
                refreshToken.RevokedByIp = ipAddress;
                _context.Update(user);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public UserDto GetCurrentUser(string token)
        {
            try
            {
                var user = _context.Users.Include(u => u.UserRefreshTokens).SingleOrDefault(u => u.UserRefreshTokens.Any(rt => rt.Token == token));

                if (user == null)
                    throw new UserNotFoundException("User with such token was not found.");

                return _mapper.MapUser(user);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetCurrentUserId(string token)
        {
            try
            {
                var user = _context.Users.Include(u => u.UserRefreshTokens).SingleOrDefault(u => u.UserRefreshTokens.Any(rt => rt.Token == token));

                if (user == null)
                    throw new Exception("User with such token was not found.");

                return user.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(JwtTokenExpiresIn),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private RefreshToken GenerateRefreshToken(string ipAddress)
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
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

        private TokensPair GenerateAndSaveTokensPair(User user, string ipAddress)
        {
            // generate jwt and refresh tokens
            var jwtToken = GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken(ipAddress);

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
            _context.Update(user);
            _context.SaveChanges();

            return new TokensPair(jwtToken, refreshToken);
        }
    }
}
