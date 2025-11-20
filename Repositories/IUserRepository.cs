using Kruggers_Backend.Data;
using Kruggers_Backend.Data.RequestDTOS;
using Kruggers_Backend.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace Kruggers_Backend.Repositories;

public interface IUserRepository
{

    Task<User?> GetUserByIdAsync(int id);
    Task<UserDTO?> GetUserByUserNameAsync(string? username);
    Task<IEnumerable<UserDTO>>GetAllUsersAsync();
    Task<bool> UpdateUserAsync(User userToUpdate, UserUpdateDTO patchedDto,IFormFile? profileImage);
    Task<Role?> GetRoleByNameAsync(string roleName);
    Task<IEnumerable<User>> GetAllCreators();
    Task<IEnumerable<User>> GetAllConsumers();
}