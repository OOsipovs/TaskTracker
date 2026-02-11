using System;
using System.ComponentModel.DataAnnotations;
using TaskService.Domain.Entities;

namespace TaskService.Application.DTOs;

public class UpdateTaskRequest
{
    [Required(ErrorMessage = "Title is required")]
    [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
    public string Title { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
    public string? Description { get; set; }

    [Required]
    public TaskPriority Priority { get; set; }

    public DateTime? DueDate { get; set; }

    [Required]
    public bool IsCompleted { get; set; }
}
