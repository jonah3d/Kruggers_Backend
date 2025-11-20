using Kruggers_Backend.Configuration;
using Kruggers_Backend.Data;
using Kruggers_Backend.Data.RequestDTOS;
using Kruggers_Backend.Models;
using Kruggers_Backend.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace Kruggers_Backend.Service;

public class UserService : IUserRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly CloudinaryService _cloudinaryService;

    public UserService(ApplicationDbContext context, IPasswordHasher<User> passwordHasher, CloudinaryService cloudinaryService)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _cloudinaryService = cloudinaryService;
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<bool> UpdateUserAsync(User userToUpdate, UserUpdateDTO patchedDto, IFormFile? profileImage = null)
    {

        var dbUser = await _context.Users.Include(U => U.Role).SingleOrDefaultAsync(u => u.Id == userToUpdate.Id);


        dbUser.Name = patchedDto.Name;
        dbUser.LastName = patchedDto.LastName;
        dbUser.Email = patchedDto.Email;
        dbUser.Phone = patchedDto.Phone;
        dbUser.DateOfBirth = patchedDto.DateOfBirth;

        if (profileImage != null)
        {
            bool deletetransaction = false;
            bool uploadtransaction = false;
            if (!string.IsNullOrEmpty(dbUser.ProfileImageId))
            {
                deletetransaction = await _cloudinaryService.DeleteImageAsync(dbUser.ProfileImageId);
            }
            var (imageUrl, publicId) = await _cloudinaryService.UploadImageAsync(profileImage);
            if (!string.IsNullOrEmpty(imageUrl))
            {
                dbUser.ProfileImage = imageUrl;
                dbUser.ProfileImageId = publicId;
                uploadtransaction = true;
            }

            if (!deletetransaction && !uploadtransaction)
            {
                return false;
            }

        }


        if (!string.IsNullOrEmpty(patchedDto.Password))
        {

            dbUser.HashedPassword = _passwordHasher.HashPassword(dbUser, patchedDto.Password);
        }



        if (dbUser.Role.RoleType.ToString() != patchedDto.UserRole)
        {
            var newRole = await GetRoleByNameAsync(patchedDto.UserRole);
            if (newRole == null)
            {
                return false;
            }
            dbUser.RoleId = newRole.Id;
        }


        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException)
        {

            return false;
        }
    }

    public async Task<Role?> GetRoleByNameAsync(string roleName)
    {
        return await _context.Roles
            .SingleOrDefaultAsync(r => r.RoleType.ToUpper() == roleName.ToUpper());
    }

    public async Task<UserDTO?> GetUserByUserNameAsync(string? username)
    {
        if (string.IsNullOrEmpty(username))
            return null;

        try
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Username.ToUpper() == username.ToUpper());

            if (user == null)
            {
                return null;
            }

            var userDto = new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email,
                Username = user.Username,
                Phone = user.Phone,
                DateOfBirth = user.DateOfBirth,
                Role = new RoleDTO
                {
                    Id = user.Role.Id,
                    RoleType = user.Role.RoleType
                },
                ProfileImage = user.ProfileImage
            };

            return userDto;
        }
        catch (Exception)
        {
            return null;

        }
    }

    public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
    {
        return await _context.Users
            .Include(u => u.Role)
            .Select(user => new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email,
                Username = user.Username,
                Phone = user.Phone,
                DateOfBirth = user.DateOfBirth,
                Role = new RoleDTO
                {
                    Id = user.Role.Id,
                    RoleType = user.Role.RoleType
                },
                ProfileImage = user.ProfileImage
            }).OrderBy(u => u.Id)
            .ToListAsync();
    }

    public async Task<IEnumerable<User>> GetAllCreators()
    {

        return await _context.Users
            .Include(u => u.Role)
            .Where(u => u.RoleId == 1)
            .OrderBy(u => u.Id)
            .ToListAsync();
    }

    public async Task<IEnumerable<User>> GetAllConsumers()
    {
        return await _context.Users
         .Include(u => u.Role)
         .Where(u=> u.RoleId == 2)
         .OrderBy(u => u.Id)
         .ToListAsync();
    }
}