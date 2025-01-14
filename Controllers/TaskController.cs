using EQ_Internship.Contract;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/tasks")]
public class TaskController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TaskController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet("GetTasks")]
    public async Task<IActionResult> GetTasks(int userId)
    {
        var tasks = await _taskService.GetTasksByUserIdAsync(userId);
        return Ok(tasks);
    }
    [HttpGet("GetTaskbyID")]
    public async Task<IActionResult> GetTask(int id)
    {
        var task = await _taskService.GetTaskByIdAsync(id); // Ensure this is returning the correct task
        if (task == null)
        {
            return NotFound(); // Return 404 if task not found
        }
        return Ok(task); // Return the task
    }


    [HttpPost("CreateTask")]
    public async Task<IActionResult> CreateTask(string title, string description, string status, string priority, DateTime deadline, int userId)
    {
        var result = await _taskService.CreateTaskAsync(title, description, status, priority, deadline, userId);
        return Ok(result);
    }

    [HttpPut("UpdateTask")]
    public async Task<IActionResult> UpdateTask(int id, string title, string description, string status, string priority, DateTime deadline)
    {
        var result = await _taskService.UpdateTaskAsync(id, title, description, status, priority, deadline);
        return Ok(result);
    }

    [HttpDelete("DeleteTask")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var result = await _taskService.DeleteTaskAsync(id);
        return Ok(result);
    }
}
