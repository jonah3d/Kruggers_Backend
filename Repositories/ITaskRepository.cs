using Kruggers_Backend.Data;
using Kruggers_Backend.Models;

namespace Kruggers_Backend.Repositories
{
    public interface ITaskRepository
    {
        Task<bool> CreateTaskAsync(ImageTask taskRequest);
        Task<IEnumerable<ImageTask>> GetAllTasksAsync();
        Task<int> GetTaskByIdAsync(int taskId);
        
        Task<IEnumerable<ImageTask>> GetCreatorAssignedTaskAsync(string creatorUsername);
        Task<IEnumerable<ImageTask>> GetConsumerRequestedTaskAsync(string consumerUsername);
       // Task<Task> UpdateTaskStatusAsync(int taskId, int statusId);
    }
}
