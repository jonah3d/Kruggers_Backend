using Kruggers_Backend.Data;
using Kruggers_Backend.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace Kruggers_Backend.Repositories;

public interface IUserRepository
{

    Task<User?> GetUserByIdAsync(int id);
    Task<bool> UpdateUserAsync(User userToUpdate, UserUpdateDTO patchedDto);
    Task<Role?> GetRoleByNameAsync(string roleName);
}