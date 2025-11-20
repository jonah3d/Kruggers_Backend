using Kruggers_Backend.Data;
using Kruggers_Backend.Data.RequestDTOS;
using Kruggers_Backend.Data.ResponseDTOS;
using Kruggers_Backend.Models;
using Kruggers_Backend.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

namespace Kruggers_Backend.Controllers
{
    [Route("api/v1/image")]
    [ApiController]
    public class ImageTaskController : ControllerBase
    {
        private readonly ITaskRepository taskRepository;
        private readonly IUserRepository userRepository;

        public ImageTaskController(ITaskRepository taskRepository, IUserRepository userRepository)
        {
            this.taskRepository = taskRepository;
            this.userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTasks()
        {
            try
            {
                var tasks = await taskRepository.GetAllTasksAsync();
                if (tasks == null || !tasks.Any())
                {
                    return NotFound("No tasks found.");
                }
                List<ImageTaskDTO> taskDTOs = new List<ImageTaskDTO>();

                foreach (var item in tasks)
                {
                    taskDTOs.Add(new ImageTaskDTO
                    {
                        Consumer = new ConsumerDTO
                        {

                            Id = item.Consumer.Id,
                            UserName = item.Consumer.Username,
                            Name = item.Consumer.Name,
                            LastName = item.Consumer.LastName
                        },
                        Creator = new CreatorDTO
                        {
                            Id = item.Creator.Id,
                            UserName = item.Creator.Username,
                            Name = item.Creator.Name,
                            LastName = item.Creator.LastName
                        },
                        Description = item.Description,
                        CreatedDate = item.CreatedDate,
                        UpdatedDate = item.UpdatedDate,
                        ImageUrl = item.ImageUrl,
                        Status = new StatusDTO
                        {
                            Id = item.Status.Id,
                            StatusType = item.Status.StatusType.ToString()
                        }
                    });
                }
                return Ok(taskDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }


        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] ImageTaskRequestDTO taskRequest)
        {
            if (taskRequest == null)
            {
                return BadRequest("Task request is null.");
            }
            try
            {
                // var userToken = taskRequest.Token;

                ImageTask task = new ImageTask
                {
                    ConsumerId = taskRequest.ConsumerId,
                    CreatorId = taskRequest.CreatorId ?? await GetRandomCreator(),
                    Description = taskRequest.Description,
                };


                var result = await taskRepository.CreateTaskAsync(task);
                if (!result)
                {
                    return StatusCode(500, "A problem happened while handling your request.");
                }
                return Ok("Task created successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize]
        [HttpGet("consumer/{username}")]
        public async Task<IActionResult> GetConsumerTasks_Public([FromRoute] string username)
        {
          var result =   await taskRepository.GetConsumerRequestedTaskAsync(username);
            if(result == null || !result.Any())
            {
                return NotFound("User Has No Requested Task");
            }
            List<ConsumerTaskResponseDTO> taskDTOs = new List<ConsumerTaskResponseDTO>();
            foreach (var item in result)
            {
                taskDTOs.Add(new ConsumerTaskResponseDTO
                {
                    RequestedBy = item.Consumer.Username,
                    AssignedTo = item.Creator.Username,
                    Description = item.Description,
                    CreatedDate = item.CreatedDate,
                    UpdatedDate = item.UpdatedDate,
                    Status = new StatusDTO
                    {
                        Id = item.Status.Id,
                        StatusType = item.Status.StatusType.ToString()
                    }
                });
            }

            return Ok(taskDTOs);
        }

        [Authorize]
        [HttpGet("creator/{username}")]
        public async Task<IActionResult> GetCreatorTasks_Public([FromRoute] string username)
        {
            var result = await taskRepository.GetCreatorAssignedTaskAsync(username);
            if (result == null || !result.Any())
            {
                return NotFound("User Has No Assigned Task");
            }
            List<CreatorTaskResponseDTO> taskDTOs = new List<CreatorTaskResponseDTO>();
            foreach (var item in result)
            {
                taskDTOs.Add(new CreatorTaskResponseDTO
                {
                    TaskId = item.Id,
                    AssignedTo = item.Creator.Username,
                    RequestedBy = item.Consumer.Username,
                    Description = item.Description,
                    CreatedDate = item.CreatedDate,
                    UpdatedDate = item.UpdatedDate,
                    Status = new StatusDTO
                    {
                        Id = item.Status.Id,
                        StatusType = item.Status.StatusType.ToString()
                    }
                });
            }
            return Ok(taskDTOs);
        }


        [Authorize]
        [HttpGet("consumer/tasks")]
        public async Task<IActionResult> GetConsumerTasks()
        {
            
       
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value ?? User.FindFirst("role")?.Value;

            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized("Invalid Token: Username missing.");
            }

            if (role != "CONSUMER")
            {
                return StatusCode(403, "Forbidden: Only Consumers can access this endpoint.");
            }

            var result = await taskRepository.GetConsumerRequestedTaskAsync(username);

            if (result == null || !result.Any())
            {
                return NotFound("You have no requested tasks.");
            }

            List<ConsumerTaskResponseDTO> taskDTOs = new List<ConsumerTaskResponseDTO>();
            foreach (var item in result)
            {
                taskDTOs.Add(new ConsumerTaskResponseDTO
                {
                    TaskId = item.Id,
                    RequestedBy = item.Consumer.Username,
                    AssignedTo = item.Creator.Username,
                    Description = item.Description,
                    CreatedDate = item.CreatedDate,
                    UpdatedDate = item.UpdatedDate,
                    Status = new StatusDTO
                    {
                        Id = item.Status.Id,
                        StatusType = item.Status.StatusType.ToString()
                    }
                });
            }

            return Ok(taskDTOs);
        }

        [Authorize]
        [HttpGet("creator/tasks")]
        public async Task<IActionResult> GetMyCreatorTasks()
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value ?? User.FindFirst("role")?.Value;


            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized("Invalid Token: Username missing.");
            }

            if (role != "CREATOR")
            {
                return StatusCode(403, "Forbidden: Only Creators can access this endpoint.");
            }

     
            var result = await taskRepository.GetCreatorAssignedTaskAsync(username);

            if (result == null || !result.Any())
            {
                return NotFound("You have no assigned tasks.");
            }

            List<CreatorTaskResponseDTO> taskDTOs = new List<CreatorTaskResponseDTO>();
            foreach (var item in result)
            {
                taskDTOs.Add(new CreatorTaskResponseDTO
                {
                    AssignedTo = item.Creator.Username,
                    RequestedBy = item.Consumer.Username,
                    Description = item.Description,
                    CreatedDate = item.CreatedDate,
                    UpdatedDate = item.UpdatedDate,
                    Status = new StatusDTO
                    {
                        Id = item.Status.Id,
                        StatusType = item.Status.StatusType.ToString()
                    }
                });
            }
            return Ok(taskDTOs);
        }


        private async Task<int> GetRandomCreator()
        {
            var creators = await userRepository.GetAllCreators();
            var ids = creators.Select(c => c.Id).ToList();

            if (ids.Count == 0)
                throw new InvalidOperationException("No creators found.");

            var rand = new Random();
            int index = rand.Next(ids.Count);
            return ids[index];
        }
    }
}