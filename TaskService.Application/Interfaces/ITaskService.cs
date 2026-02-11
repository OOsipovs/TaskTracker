using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskService.Application.DTOs;

namespace TaskService.Application.Interfaces
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskItemDto>> GetAllTasksAsync(int userId);
        Task<TaskItemDto?> GetTaskByIdAsync(int id, int userId);
        Task<TaskItemDto> CreateTaskAsync(CreateTaskRequest request, int userId);
        Task<TaskItemDto?> UpdateTaskAsync(int id, UpdateTaskRequest request, int userId);
        Task<bool> DeleteTaskAsync(int id, int userId);
    }
}
