using Kruggers_Backend.Data;
using Kruggers_Backend.Repositories;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;



namespace Kruggers_Backend.Controllers;

[ApiController]
[Route("/api/v1/user")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet("{username}")]
    public async Task<IActionResult> GetUserByUsername(string username)
    {
        if(string.IsNullOrWhiteSpace(username))
        {
            return BadRequest("Username cannot be null or empty.");
        }
        try
        {
            var user = await _userRepository.GetUserByUserNameAsync(username);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        
        }catch(Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }


    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        try
        {
            var users = await _userRepository.GetAllUsersAsync();
            if (users == null) {
                return NotFound();
            }
            return Ok(users);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }


    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchUser(
        int id,
        [FromForm] string? patchJson,  
        [FromForm] IFormFile? profileImage) 
    {
        
        if (string.IsNullOrWhiteSpace(patchJson) && profileImage == null)
        {
            return BadRequest("No update data provided.");
        }

        var dbUser = await _userRepository.GetUserByIdAsync(id);
        if (dbUser == null)
        {
            return NotFound();
        }

        
        var userToPatch = new UserUpdateDTO
        {
            Name = dbUser.Name,
            LastName = dbUser.LastName,
            Email = dbUser.Email,
            Phone = dbUser.Phone,
            DateOfBirth = dbUser.DateOfBirth,
            ProfileImage = dbUser.ProfileImage,
            UserRole = dbUser.Role.RoleType.ToString(),
            Password = null
        };

        
        if (!string.IsNullOrWhiteSpace(patchJson))
        {
            JsonPatchDocument<UserUpdateDTO>? patchDoc;
            try
            {
                patchDoc = JsonConvert.DeserializeObject<JsonPatchDocument<UserUpdateDTO>>(patchJson);
            }
            catch (JsonException ex)
            {
                return BadRequest($"Invalid JSON patch document: {ex.Message}");
            }

            if (patchDoc == null)
            {
                return BadRequest("Failed to parse patch document.");
            }

            patchDoc.ApplyTo(userToPatch, ModelState);

            if (!TryValidateModel(userToPatch))
            {
                return ValidationProblem(ModelState);
            }
        }

       
        var updateResult = await _userRepository.UpdateUserAsync(dbUser, userToPatch, profileImage);

        if (!updateResult)
        {
            ModelState.AddModelError("Error", "Unable to update user");
            return BadRequest(ModelState);
        }

        return NoContent(); 
    }


}