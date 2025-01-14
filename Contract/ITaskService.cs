
using EQ_Internship.Models;

namespace EQ_Internship.Contract
{
    public interface ITaskService
    {
        Task<IEnumerable<Tasks>> GetTasksByUserIdAsync(int userId);
        Task<Tasks> GetTaskByIdAsync(int taskId);
        Task<string> CreateTaskAsync(string title, string description, string status, string priority, DateTime deadline, int userId);
        Task<string> UpdateTaskAsync(int taskId, string title, string description, string status, string priority, DateTime deadline);
        Task<string> DeleteTaskAsync(int taskId);
    }

}
