using Kruggers_Backend.Configuration;
using Kruggers_Backend.Data;
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

    public UserService(ApplicationDbContext context,IPasswordHasher<User> passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _context.Users
            .Include(u => u.Role) 
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<bool> UpdateUserAsync(User userToUpdate, UserUpdateDTO patchedDto)
    {
   
        var dbUser = await _context.Users.Include(U=> U.Role).SingleOrDefaultAsync(u => u.Id == userToUpdate.Id);
        
        
        dbUser.Name = patchedDto.Name;
        dbUser.LastName = patchedDto.LastName;
        dbUser.Email = patchedDto.Email;
        dbUser.Phone = patchedDto.Phone;
        dbUser.DateOfBirth = patchedDto.DateOfBirth;
        dbUser.ProfileImage = patchedDto.ProfileImage;

     
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
}