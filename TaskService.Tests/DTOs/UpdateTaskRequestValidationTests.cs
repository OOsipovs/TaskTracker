using System.ComponentModel.DataAnnotations;
using TaskService.Application.DTOs;
using TaskService.Domain.Entities;
using Xunit;

namespace TaskService.Tests.DTOs;

public class UpdateTaskRequestValidationTests
{
    private List<ValidationResult> ValidateModel(object model)
    {
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(model, null, null);
        Validator.TryValidateObject(model, validationContext, validationResults, true);
        return validationResults;
    }

    [Fact]
    public void UpdateTaskRequest_WithValidData_PassesValidation()
    {
        // Arrange
        var request = new UpdateTaskRequest
        {
            Title = "Updated Task Title",
            Description = "Updated description",
            Priority = TaskPriority.High,
            DueDate = DateTime.UtcNow.AddDays(7),
            IsCompleted = false
        };

        // Act
        var validationResults = ValidateModel(request);

        // Assert
        Assert.Empty(validationResults);
    }

    [Fact]
    public void UpdateTaskRequest_WithEmptyTitle_FailsValidation()
    {
        // Arrange
        var request = new UpdateTaskRequest
        {
            Title = "",
            Description = "Description",
            Priority = TaskPriority.Medium,
            IsCompleted = true
        };

        // Act
        var validationResults = ValidateModel(request);

        // Assert
        Assert.NotEmpty(validationResults);
        Assert.Contains(validationResults, v => v.ErrorMessage == "Title is required");
    }

    [Fact]
    public void UpdateTaskRequest_WithTitleExceeding200Characters_FailsValidation()
    {
        // Arrange
        var request = new UpdateTaskRequest
        {
            Title = new string('X', 201),
            Description = "Description",
            Priority = TaskPriority.Low,
            IsCompleted = false
        };

        // Act
        var validationResults = ValidateModel(request);

        // Assert
        Assert.NotEmpty(validationResults);
        Assert.Contains(validationResults, v => v.ErrorMessage == "Title cannot exceed 200 characters");
    }

    [Fact]
    public void UpdateTaskRequest_WithDescriptionExceeding1000Characters_FailsValidation()
    {
        // Arrange
        var request = new UpdateTaskRequest
        {
            Title = "Valid Title",
            Description = new string('Y', 1001),
            Priority = TaskPriority.Medium,
            IsCompleted = true
        };

        // Act
        var validationResults = ValidateModel(request);

        // Assert
        Assert.NotEmpty(validationResults);
        Assert.Contains(validationResults, v => v.ErrorMessage == "Description cannot exceed 1000 characters");
    }

    [Fact]
    public void UpdateTaskRequest_WithCompletedTask_PassesValidation()
    {
        // Arrange
        var request = new UpdateTaskRequest
        {
            Title = "Completed Task",
            Description = "This task is done",
            Priority = TaskPriority.High,
            DueDate = DateTime.UtcNow.AddDays(-1),
            IsCompleted = true
        };

        // Act
        var validationResults = ValidateModel(request);

        // Assert
        Assert.Empty(validationResults);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void UpdateTaskRequest_WithIsCompletedFlag_PassesValidation(bool isCompleted)
    {
        // Arrange
        var request = new UpdateTaskRequest
        {
            Title = "Task",
            Priority = TaskPriority.Medium,
            IsCompleted = isCompleted
        };

        // Act
        var validationResults = ValidateModel(request);

        // Assert
        Assert.Empty(validationResults);
    }
}
