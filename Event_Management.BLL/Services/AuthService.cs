using Event_Management.BLL.DTOs.Auth;
using Event_Management.BLL.Interfaces.Repositories;
using Event_Management.BLL.Interfaces.Services;
using Event_Management.CrossCutting.Entities;
using Event_Management.CrossCutting.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IOrganizerRepository _organizerRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenGenerator _tokenGenerator;

        public AuthService(
            IUserRepository userRepository,
            IOrganizerRepository organizerRepository,
            IPasswordHasher passwordHasher,
            IJwtTokenGenerator tokenGenerator)
        {
            _userRepository = userRepository;
            _organizerRepository = organizerRepository;
            _passwordHasher = passwordHasher;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<AuthResponse> Register(RegisterRequest request)
        {
            if (await _userRepository.EmailExistsAsync(request.Email))
                throw new InvalidOperationException("User with this email already exists");

            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                PasswordHash = _passwordHasher.HashPassword(request.Password),
                Role = (UserRole)request.Role
            };

            await _userRepository.AddAsync(user);

            if (user.Role == UserRole.Organizer)
            {
                var organizer = new Organizer
                {
                    UserId = user.Id,
                    CompanyName = request.Name,
                    Description = string.Empty,
                    Verified = false
                };
                await _organizerRepository.AddAsync(organizer);
            }

            var token = _tokenGenerator.GenerateToken(user);

            return new AuthResponse
            {
                UserId = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role.ToString(),
                Token = token
            };
        }

        public async Task<AuthResponse> Login(LoginRequest request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null || !_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
                throw new InvalidOperationException("Invalid email or password");

            var token = _tokenGenerator.GenerateToken(user);

            return new AuthResponse
            {
                UserId = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role.ToString(),
                Token = token
            };
        }
    }
}
