using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Kruggers_Backend.Configuration;
using Kruggers_Backend.Data;
using Kruggers_Backend.Models;
using Kruggers_Backend.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Kruggers_Backend.Service;

public class UserAuthService : IUserAuthRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IConfiguration _configuration;

    public UserAuthService(ApplicationDbContext context, IPasswordHasher<User> passwordHasher,IConfiguration configuration)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _configuration = configuration;
    }
    
    public async Task<bool> CreateUserAsync(UserRegistrationDTO userRegistrationDto)
    {
       

        try
        {
            var role = await _context.Roles
                .SingleOrDefaultAsync(r => r.RoleType.ToUpper() == userRegistrationDto.UserRole.ToUpper());

            if (role == null)
            {
              //  throw new InvalidOperationException($"Role '{userRegistrationDto.UserRole}' not found.");
              return  false;
            }

            var userToDb = new User
            {
                Name = userRegistrationDto.Firstname,
                LastName = userRegistrationDto.Lastname,
                Email = userRegistrationDto.Email,
                Username = userRegistrationDto.Username,
                RoleId = role.Id
            };

            userToDb.HashedPassword = _passwordHasher.HashPassword(userToDb, userRegistrationDto.Password);

            await _context.Users.AddAsync(userToDb);
            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine($"[CreateUserAsync] Error: {e.Message}");
            throw;
        }
    }

    public async Task<UserLoginResponseDTO> LoginUser(UserLoginDTO userLoginDto)
    {
       

        try
        {
            User userFromDb = await _context.Users.Include(U => U.Role)
                .SingleOrDefaultAsync(u => u.Username == userLoginDto.Username);
            var result =
                _passwordHasher.VerifyHashedPassword(userFromDb, userFromDb.HashedPassword, userLoginDto.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                return null;
            }

            var jwtSettings = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userFromDb.Username),
                new Claim(JwtRegisteredClaimNames.Email, userFromDb.Email),
                new Claim("role", userFromDb.Role.RoleType.ToString() ?? "CONSUMER"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["ExpiresInMinutes"])),
                signingCredentials: creds
            );

            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            UserLoginResponseDTO userLoginResponseDTO = new UserLoginResponseDTO()
            {
                UserDto = new UserDTO()
                {
                    Id = userFromDb.Id,
                    Name = userFromDb.Name,
                    LastName = userFromDb.LastName,
                    Email = userFromDb.Email,
                    Username = userFromDb.Username,
                    Phone = userFromDb.Phone,
                    DateOfBirth = userFromDb.DateOfBirth,
                    ProfileImage = userFromDb.ProfileImage,
                    Role = new RoleDTO
                    {
                        Id = userFromDb.Role.Id,
                        RoleType = userFromDb.Role.RoleType
                    }
                },
                Token = jwtToken
            };

            return userLoginResponseDTO;

        }



        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}