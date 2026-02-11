using System.ComponentModel.DataAnnotations;
using TaskService.Application.DTOs;
using TaskService.Domain.Entities;
using Xunit;

namespace TaskService.Tests.DTOs;

public class CreateTaskRequestValidationTests
{
    private List<ValidationResult> ValidateModel(object model)
    {
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(model, null, null);
        Validator.TryValidateObject(model, validationContext, validationResults, true);
        return validationResults;
    }

    [Fact]
    public void CreateTaskRequest_WithValidData_PassesValidation()
    {
        // Arrange
        var request = new CreateTaskRequest
        {
            Title = "Valid Task Title",
            Description = "Valid description",
            Priority = TaskPriority.Medium,
            DueDate = DateTime.UtcNow.AddDays(7)
        };

        // Act
        var validationResults = ValidateModel(request);

        // Assert
        Assert.Empty(validationResults);
    }

    [Fact]
    public void CreateTaskRequest_WithEmptyTitle_FailsValidation()
    {
        // Arrange
        var request = new CreateTaskRequest
        {
            Title = "",
            Description = "Description",
            Priority = TaskPriority.High
        };

        // Act
        var validationResults = ValidateModel(request);

        // Assert
        Assert.NotEmpty(validationResults);
        Assert.Contains(validationResults, v => v.ErrorMessage == "Title is required");
    }

    [Fact]
    public void CreateTaskRequest_WithNullTitle_FailsValidation()
    {
        // Arrange
        var request = new CreateTaskRequest
        {
            Title = null!,
            Description = "Description",
            Priority = TaskPriority.High
        };

        // Act
        var validationResults = ValidateModel(request);

        // Assert
        Assert.NotEmpty(validationResults);
        Assert.Contains(validationResults, v => v.ErrorMessage == "Title is required");
    }

    [Fact]
    public void CreateTaskRequest_WithWhitespaceTitle_FailsValidation()
    {
        // Arrange
        var request = new CreateTaskRequest
        {
            Title = "   ",
            Description = "Description",
            Priority = TaskPriority.High
        };

        // Act
        var validationResults = ValidateModel(request);

        // Assert
        Assert.NotEmpty(validationResults);
        Assert.Contains(validationResults, v => v.ErrorMessage == "Title is required");
    }

    [Fact]
    public void CreateTaskRequest_WithTitleExceeding200Characters_FailsValidation()
    {
        // Arrange
        var request = new CreateTaskRequest
        {
            Title = new string('A', 201), // 201 characters
            Description = "Description",
            Priority = TaskPriority.Medium
        };

        // Act
        var validationResults = ValidateModel(request);

        // Assert
        Assert.NotEmpty(validationResults);
        Assert.Contains(validationResults, v => v.ErrorMessage == "Title cannot exceed 200 characters");
    }

    [Fact]
    public void CreateTaskRequest_WithTitleExactly200Characters_PassesValidation()
    {
        // Arrange
        var request = new CreateTaskRequest
        {
            Title = new string('A', 200), // Exactly 200 characters
            Description = "Description",
            Priority = TaskPriority.Medium
        };

        // Act
        var validationResults = ValidateModel(request);

        // Assert
        Assert.Empty(validationResults);
    }

    [Fact]
    public void CreateTaskRequest_WithDescriptionExceeding1000Characters_FailsValidation()
    {
        // Arrange
        var request = new CreateTaskRequest
        {
            Title = "Valid Title",
            Description = new string('B', 1001), // 1001 characters
            Priority = TaskPriority.Low
        };

        // Act
        var validationResults = ValidateModel(request);

        // Assert
        Assert.NotEmpty(validationResults);
        Assert.Contains(validationResults, v => v.ErrorMessage == "Description cannot exceed 1000 characters");
    }

    [Fact]
    public void CreateTaskRequest_WithDescriptionExactly1000Characters_PassesValidation()
    {
        // Arrange
        var request = new CreateTaskRequest
        {
            Title = "Valid Title",
            Description = new string('B', 1000), // Exactly 1000 characters
            Priority = TaskPriority.Low
        };

        // Act
        var validationResults = ValidateModel(request);

        // Assert
        Assert.Empty(validationResults);
    }

    [Fact]
    public void CreateTaskRequest_WithNullDescription_PassesValidation()
    {
        // Arrange
        var request = new CreateTaskRequest
        {
            Title = "Valid Title",
            Description = null,
            Priority = TaskPriority.Medium
        };

        // Act
        var validationResults = ValidateModel(request);

        // Assert
        Assert.Empty(validationResults);
    }

    [Fact]
    public void CreateTaskRequest_WithEmptyDescription_PassesValidation()
    {
        // Arrange
        var request = new CreateTaskRequest
        {
            Title = "Valid Title",
            Description = "",
            Priority = TaskPriority.High
        };

        // Act
        var validationResults = ValidateModel(request);

        // Assert
        Assert.Empty(validationResults);
    }

    [Theory]
    [InlineData(TaskPriority.Low)]
    [InlineData(TaskPriority.Medium)]
    [InlineData(TaskPriority.High)]
    public void CreateTaskRequest_WithValidPriority_PassesValidation(TaskPriority priority)
    {
        // Arrange
        var request = new CreateTaskRequest
        {
            Title = "Valid Title",
            Description = "Description",
            Priority = priority
        };

        // Act
        var validationResults = ValidateModel(request);

        // Assert
        Assert.Empty(validationResults);
    }

    [Fact]
    public void CreateTaskRequest_WithFutureDueDate_PassesValidation()
    {
        // Arrange
        var request = new CreateTaskRequest
        {
            Title = "Valid Title",
            Description = "Description",
            Priority = TaskPriority.Medium,
            DueDate = DateTime.UtcNow.AddDays(30)
        };

        // Act
        var validationResults = ValidateModel(request);

        // Assert
        Assert.Empty(validationResults);
    }

    [Fact]
    public void CreateTaskRequest_WithPastDueDate_PassesValidation()
    {
        // Arrange
        var request = new CreateTaskRequest
        {
            Title = "Valid Title",
            Description = "Description",
            Priority = TaskPriority.High,
            DueDate = DateTime.UtcNow.AddDays(-5)
        };

        // Act
        var validationResults = ValidateModel(request);

        // Assert
        Assert.Empty(validationResults); // DueDate validation is not enforced in current model
    }

    [Fact]
    public void CreateTaskRequest_WithNullDueDate_PassesValidation()
    {
        // Arrange
        var request = new CreateTaskRequest
        {
            Title = "Valid Title",
            Description = "Description",
            Priority = TaskPriority.Medium,
            DueDate = null
        };

        // Act
        var validationResults = ValidateModel(request);

        // Assert
        Assert.Empty(validationResults);
    }

    [Fact]
    public void CreateTaskRequest_WithMinimalValidData_PassesValidation()
    {
        // Arrange
        var request = new CreateTaskRequest
        {
            Title = "T" // Minimal valid title
        };

        // Act
        var validationResults = ValidateModel(request);

        // Assert
        Assert.Empty(validationResults);
    }

    [Fact]
    public void CreateTaskRequest_WithAllFieldsPopulated_PassesValidation()
    {
        // Arrange
        var request = new CreateTaskRequest
        {
            Title = "Complete Task",
            Description = "This is a complete task with all fields populated",
            Priority = TaskPriority.High,
            DueDate = DateTime.UtcNow.AddDays(14)
        };

        // Act
        var validationResults = ValidateModel(request);

        // Assert
        Assert.Empty(validationResults);
    }
}
