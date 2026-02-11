import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { TaskService } from '../../../core/services/task.service';
import { Task } from '../../../core/models/task.model';
import { TaskPriority } from '../../../core/models/task-priority.enum';
import { TaskFormComponent } from '../task-form/task-form.component';

@Component({
  selector: 'app-task-list',
  standalone: true,
  imports: [CommonModule, TaskFormComponent],
  templateUrl: './task-list.component.html',
  styleUrls: ['./task-list.component.scss']
})
export class TaskListComponent implements OnInit {
  tasks: Task[] = [];
  isLoading: boolean = false;
  errorMessage: string = '';
  showTaskForm: boolean = false;
  editingTask: Task | null = null;

  TaskPriority = TaskPriority;

  constructor(
    private taskService: TaskService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadTasks();
  }

  loadTasks(): void {
    this.isLoading = true;
    this.taskService.getAllTasks().subscribe({
      next: (tasks) => {
        this.tasks = tasks;
        this.isLoading = false;
      },
      error: (error) => {
        this.errorMessage = 'Failed to load tasks';
        this.isLoading = false;
        console.error('Error loading tasks:', error);
      }
    });
  }

  getPriorityLabel(priority: TaskPriority): string {
    return TaskPriority[priority];
  }

  getPriorityClass(priority: TaskPriority): string {
    switch (priority) {
      case TaskPriority.High: return 'priority-high';
      case TaskPriority.Medium: return 'priority-medium';
      case TaskPriority.Low: return 'priority-low';
      default: return '';
    }
  }

  toggleTaskCompletion(task: Task): void {
    if (!task.id) return;

    const updatedTask = { ...task, isCompleted: !task.isCompleted };
    
    this.taskService.updateTask(task.id, {
      title: task.title,
      description: task.description,
      priority: task.priority,
      dueDate: task.dueDate,
      isCompleted: updatedTask.isCompleted
    }).subscribe({
      next: () => {
        this.loadTasks();
      },
      error: (error) => {
        console.error('Error updating task:', error);
        this.errorMessage = 'Failed to update task';
      }
    });
  }

  deleteTask(taskId: number | undefined): void {
    if (!taskId || !confirm('Are you sure you want to delete this task?')) {
      return;
    }

    this.taskService.deleteTask(taskId).subscribe({
      next: () => {
        this.loadTasks();
      },
      error: (error) => {
        console.error('Error deleting task:', error);
        this.errorMessage = 'Failed to delete task';
      }
    });
  }

  openCreateForm(): void {
    this.editingTask = null;
    this.showTaskForm = true;
  }

  openEditForm(task: Task): void {
    this.editingTask = task;
    this.showTaskForm = true;
  }

  closeForm(): void {
    this.showTaskForm = false;
    this.editingTask = null;
  }

  onTaskSaved(): void {
    this.closeForm();
    this.loadTasks();
  }
}