using TaskService.Application.DTOs;
using TaskService.Application.Interfaces;
using TaskService.Domain.Entities;
using TaskService.Domain.Interfaces;

namespace TaskService.Application.Services;

public class TaskManagementService : ITaskService
{
    private readonly ITaskRepository _taskRepository;

    public TaskManagementService(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<IEnumerable<TaskItemDto>> GetAllTasksAsync(int userId)
    {
        var tasks = await _taskRepository.GetAllByUserIdAsync(userId);
        return tasks.Select(MapToDto);
    }

    public async Task<TaskItemDto?> GetTaskByIdAsync(int id, int userId)
    {
        var task = await _taskRepository.GetByIdAsync(id, userId);
        return task != null ? MapToDto(task) : null;
    }

    public async Task<TaskItemDto> CreateTaskAsync(CreateTaskRequest request, int userId)
    {
        var task = new TaskItem
        {
            Title = request.Title,
            Description = request.Description,
            Priority = request.Priority,
            DueDate = request.DueDate,
            UserId = userId,
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow
        };

        var createdTask = await _taskRepository.CreateAsync(task);
        return MapToDto(createdTask);
    }

    public async Task<TaskItemDto?> UpdateTaskAsync(int id, UpdateTaskRequest request, int userId)
    {
        var existingTask = await _taskRepository.GetByIdAsync(id, userId);
        
        if (existingTask == null)
        {
            return null;
        }

        existingTask.Title = request.Title;
        existingTask.Description = request.Description;
        existingTask.Priority = request.Priority;
        existingTask.DueDate = request.DueDate;
        existingTask.IsCompleted = request.IsCompleted;
        existingTask.UpdatedAt = DateTime.UtcNow;

        var updatedTask = await _taskRepository.UpdateAsync(existingTask);
        return updatedTask != null ? MapToDto(updatedTask) : null;
    }

    public async Task<bool> DeleteTaskAsync(int id, int userId)
    {
        return await _taskRepository.DeleteAsync(id, userId);
    }

    private static TaskItemDto MapToDto(TaskItem task)
    {
        return new TaskItemDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Priority = task.Priority,
            DueDate = task.DueDate,
            IsCompleted = task.IsCompleted,
            CreatedAt = task.CreatedAt,
            UpdatedAt = task.UpdatedAt
        };
    }
}
