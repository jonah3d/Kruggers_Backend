using Kruggers_Backend.Configuration;
using Kruggers_Backend.Models;
using Kruggers_Backend.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Kruggers_Backend.Service
{
    public class TaskService : ITaskRepository
    {
        private readonly ApplicationDbContext context;
        private readonly CloudinaryService cloudinaryService;

        public TaskService(ApplicationDbContext context, CloudinaryService cloudinaryService)
        {
            this.context = context;
            this.cloudinaryService = cloudinaryService;
        }
        public async Task<bool> CreateTaskAsync(ImageTask taskRequest)
        {
            if(taskRequest == null)
            {
                return false;
            }
            try
            {
                if(! await checkCreator(taskRequest.CreatorId))
                {
                    throw new Exception("You Must Assign The Task To A Creator");
                }
                taskRequest.CreatedDate = DateTime.UtcNow;
                taskRequest.StatusId = 2;
                var result = await context.ImageTasks.AddAsync(taskRequest);
                
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

       }

        private async Task<bool> checkCreator(int creatorId)
        {
            var result = await    context.Users.FirstOrDefaultAsync(u=> u.Id == creatorId && u.RoleId == 1);
            return result != null;
        }

        public async Task<IEnumerable<ImageTask>> GetAllTasksAsync()
        {
            var tasks = await context.ImageTasks.Include(t => t.Consumer)
                                                .Include(t => t.Creator)
                                                .Include(t => t.Status)
                                                .OrderBy(t => t.CreatedDate)
                                                .ToListAsync();
            return tasks;
        }

        public async Task<IEnumerable<ImageTask>> GetConsumerRequestedTaskAsync(string consumerUsername)
        {
            return await context.ImageTasks
                                .Where(t => t.Consumer.Username == consumerUsername)
                                .Include(t => t.Consumer)
                                .Include(t => t.Creator)
                                .Include(t => t.Status)
                                .OrderBy(t => t.CreatedDate)
                                .ToListAsync();
        }

        public async Task<IEnumerable<ImageTask>> GetCreatorAssignedTaskAsync(string creatorUsername)
        {
            return await context.ImageTasks
                                .Where(t => t.Creator.Username == creatorUsername)
                                .Include(t => t.Consumer)
                                .Include(t => t.Creator)
                                .Include(t => t.Status)
                                .OrderBy(t => t.CreatedDate)
                                .ToListAsync();
        }

        public Task<int> GetTaskByIdAsync(int taskId)
        {
            throw new NotImplementedException();
        }
    }
}
