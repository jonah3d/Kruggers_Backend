using Kruggers_Backend.Data;
using Kruggers_Backend.Models;
using Kruggers_Backend.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace Kruggers_Backend.Controllers
{
    [Route("api/v1/image")]
    [ApiController]
    public class ImageTaskController : ControllerBase
    {
        private readonly ITaskRepository taskRepository;
        private readonly IUserRepository userRepository;

        public ImageTaskController(ITaskRepository taskRepository,IUserRepository userRepository)
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
        public async Task<IActionResult> CreateTask([FromBody] ImageaskRequestDTO taskRequest)
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