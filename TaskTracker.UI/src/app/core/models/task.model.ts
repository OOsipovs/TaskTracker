import { TaskPriority } from './task-priority.enum';

export interface Task {
  id?: number;
  title: string;
  description?: string;
  priority: TaskPriority;
  dueDate?: Date | string;
  isCompleted: boolean;
  createdAt?: Date;
  updatedAt?: Date;
}

export interface CreateTaskRequest {
  title: string;
  description?: string;
  priority: TaskPriority;
  dueDate?: Date | string;
}

export interface UpdateTaskRequest {
  title: string;
  description?: string;
  priority: TaskPriority;
  dueDate?: Date | string;
  isCompleted: boolean;
}