using System.Data;
using EQ_Internship.Contract;
using Dapper;
using EQ_Internship.Models;

namespace EQ_Internship.Services
{
    public class TaskService : ITaskService
    {
        private readonly IDbConnection _dbConnection;

        public TaskService(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        // Get tasks by userId
        public async Task<IEnumerable<Tasks>> GetTasksByUserIdAsync(int userId)
        {
            var tasks = await _dbConnection.QueryAsync<Tasks>(
                "SELECT * FROM Tasks WHERE UserId = @UserId", new { UserId=userId });

            return tasks;
        }

        // Get a specific task by taskId
        public async Task<Tasks> GetTaskByIdAsync(int taskId)
        {
            var task = await _dbConnection.QueryFirstOrDefaultAsync<Tasks>(
                "SELECT * FROM Tasks WHERE TaskId = @TaskId", new { taskId });

            return task;
        }

        // Create a new task
        public async Task<string> CreateTaskAsync(string title, string description, string status, string priority, DateTime deadline, int userId)
        {
            var sql = "INSERT INTO Tasks (Title, Description, Status, Priority, Deadline, UserId) VALUES (@Title, @Description, @Status, @Priority, @Deadline, @UserId)";
            var result = await _dbConnection.ExecuteAsync(sql, new { title, description, status, priority, deadline, userId });

            return result > 0 ? "Task created successfully!" : "Failed to create task.";
        }

        // Update an existing task
        public async Task<string> UpdateTaskAsync(int taskId, string title, string description, string status, string priority, DateTime deadline)
        {
            var sql = "UPDATE Tasks SET Title = @Title, Description = @Description, Status = @Status, Priority = @Priority, Deadline = @Deadline WHERE TaskId = @TaskId";
            var result = await _dbConnection.ExecuteAsync(sql, new { title, description, status, priority, deadline, taskId });

            return result > 0 ? "Task updated successfully!" : "Failed to update task.";
        }

        // Delete a task
        public async Task<string> DeleteTaskAsync(int taskId)
        {
            var sql = "DELETE FROM Tasks WHERE TaskId = @TaskId";
            var result = await _dbConnection.ExecuteAsync(sql, new { taskId });

            return result > 0 ? "Task deleted successfully!" : "Failed to delete task.";
        }
    }


}

