using Moq;
using TaskService.Application.DTOs;
using TaskService.Application.Services;
using TaskService.Domain.Entities;
using TaskService.Domain.Interfaces;
using Xunit;

namespace TaskService.Tests.Services;

public class TaskManagementServiceTests
{
    private readonly Mock<ITaskRepository> _mockTaskRepository;
    private readonly TaskManagementService _taskService;

    public TaskManagementServiceTests()
    {
        _mockTaskRepository = new Mock<ITaskRepository>();
        _taskService = new TaskManagementService(_mockTaskRepository.Object);
    }

    [Fact]
    public async Task GetAllTasksAsync_ReturnsAllUserTasks()
    {
        // Arrange
        var userId = 1;
        var tasks = new List<TaskItem>
        {
            new() { Id = 1, Title = "Task 1", UserId = userId, Priority = TaskPriority.High },
            new() { Id = 2, Title = "Task 2", UserId = userId, Priority = TaskPriority.Low }
        };

        _mockTaskRepository.Setup(x => x.GetAllByUserIdAsync(userId))
            .ReturnsAsync(tasks);

        // Act
        var result = await _taskService.GetAllTasksAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task CreateTaskAsync_CreatesNewTask()
    {
        // Arrange
        var userId = 1;
        var request = new CreateTaskRequest
        {
            Title = "New Task",
            Description = "Description",
            Priority = TaskPriority.Medium,
            DueDate = DateTime.UtcNow.AddDays(7)
        };

        _mockTaskRepository.Setup(x => x.CreateAsync(It.IsAny<TaskItem>()))
            .ReturnsAsync((TaskItem t) => { t.Id = 1; return t; });

        // Act
        var result = await _taskService.CreateTaskAsync(request, userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("New Task", result.Title);
        Assert.Equal(TaskPriority.Medium, result.Priority);
        _mockTaskRepository.Verify(x => x.CreateAsync(It.IsAny<TaskItem>()), Times.Once);
    }

    [Fact]
    public async Task UpdateTaskAsync_WithValidId_UpdatesTask()
    {
        // Arrange
        var userId = 1;
        var taskId = 1;
        var existingTask = new TaskItem
        {
            Id = taskId,
            Title = "Old Title",
            UserId = userId,
            Priority = TaskPriority.Low
        };

        var updateRequest = new UpdateTaskRequest
        {
            Title = "Updated Title",
            Priority = TaskPriority.High,
            IsCompleted = true
        };

        _mockTaskRepository.Setup(x => x.GetByIdAsync(taskId, userId))
            .ReturnsAsync(existingTask);

        _mockTaskRepository.Setup(x => x.UpdateAsync(It.IsAny<TaskItem>()))
            .ReturnsAsync((TaskItem t) => t);

        // Act
        var result = await _taskService.UpdateTaskAsync(taskId, updateRequest, userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Updated Title", result.Title);
        Assert.Equal(TaskPriority.High, result.Priority);
        Assert.True(result.IsCompleted);
    }

    [Fact]
    public async Task DeleteTaskAsync_WithValidId_ReturnsTrue()
    {
        // Arrange
        var userId = 1;
        var taskId = 1;

        _mockTaskRepository.Setup(x => x.DeleteAsync(taskId, userId))
            .ReturnsAsync(true);

        // Act
        var result = await _taskService.DeleteTaskAsync(taskId, userId);

        // Assert
        Assert.True(result);
        _mockTaskRepository.Verify(x => x.DeleteAsync(taskId, userId), Times.Once);
    }

    [Fact]
    public async Task DeleteTaskAsync_WithInvalidId_ReturnsFalse()
    {
        // Arrange
        var userId = 1;
        var taskId = 999;

        _mockTaskRepository.Setup(x => x.DeleteAsync(taskId, userId))
            .ReturnsAsync(false);

        // Act
        var result = await _taskService.DeleteTaskAsync(taskId, userId);

        // Assert
        Assert.False(result);
    }
}