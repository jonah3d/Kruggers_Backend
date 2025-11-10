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

    public UserController(IUserRepository  userRepository)
    {
        _userRepository = userRepository;
    }
    
    /*[HttpPatch("{id}")]
    public async Task<IActionResult> PatchUser(int id, [FromBody] JsonPatchDocument<UserUpdateDTO> patchDoc,IFormFile? profileImage)
    {
        if (patchDoc == null)
        {
            return BadRequest();
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


        patchDoc.ApplyTo(userToPatch, ModelState);


        if (!TryValidateModel(userToPatch))
        {
            return ValidationProblem(ModelState);
        }

 
        var updateResult = await _userRepository.UpdateUserAsync(dbUser, userToPatch, profileImage);

        if (!updateResult)
        {
  
            ModelState.AddModelError("Error", "Unable to update user");
            return BadRequest(ModelState);
        }

        return NoContent(); // Success
    }*/

    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchUser(
        int id,
        [FromForm] string patchJson,
        IFormFile? profileImage)
    {
        if (string.IsNullOrWhiteSpace(patchJson))
            return BadRequest("Missing patch document.");

        JsonPatchDocument<UserUpdateDTO>? patchDoc;

        try
        {
            patchDoc = JsonConvert.DeserializeObject<JsonPatchDocument<UserUpdateDTO>>(patchJson);
        }
        catch (JsonException)
        {
            return BadRequest("Invalid JSON patch document.");
        }

        if (patchDoc == null)
            return BadRequest("Failed to parse patch document.");

        var dbUser = await _userRepository.GetUserByIdAsync(id);
        if (dbUser == null)
            return NotFound();

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

        patchDoc.ApplyTo(userToPatch, ModelState);

        if (!TryValidateModel(userToPatch))
            return ValidationProblem(ModelState);

        var updateResult = await _userRepository.UpdateUserAsync(dbUser, userToPatch, profileImage);

        if (!updateResult)
        {
            ModelState.AddModelError("Error", "Unable to update user");
            return BadRequest(ModelState);
        }

        return NoContent();
    }

}