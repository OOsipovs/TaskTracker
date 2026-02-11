import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { TaskService } from '../../../core/services/task.service';
import { Task, CreateTaskRequest, UpdateTaskRequest } from '../../../core/models/task.model';
import { TaskPriority } from '../../../core/models/task-priority.enum';

@Component({
  selector: 'app-task-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './task-form.component.html',
  styleUrls: ['./task-form.component.scss']
})
export class TaskFormComponent implements OnInit {
  @Input() task: Task | null = null;
  @Output() close = new EventEmitter<void>();
  @Output() taskSaved = new EventEmitter<void>();

  taskForm: FormGroup;
  errorMessage: string = '';
  isLoading: boolean = false;
  isEditMode: boolean = false;

  priorityOptions = [
    { value: TaskPriority.Low, label: 'Low' },
    { value: TaskPriority.Medium, label: 'Medium' },
    { value: TaskPriority.High, label: 'High' }
  ];

  constructor(
    private fb: FormBuilder,
    private taskService: TaskService
  ) {
    this.taskForm = this.fb.group({
      title: ['', [Validators.required, Validators.maxLength(200)]],
      description: ['', [Validators.maxLength(1000)]],
      priority: [TaskPriority.Medium, [Validators.required]],
      dueDate: ['']
    });
  }

  ngOnInit(): void {
    if (this.task) {
      this.isEditMode = true;
      this.loadTaskData();
    }
  }

  loadTaskData(): void {
    if (!this.task) return;

    this.taskForm.patchValue({
      title: this.task.title,
      description: this.task.description || '',
      priority: this.task.priority,
      dueDate: this.task.dueDate ? this.formatDateForInput(new Date(this.task.dueDate)) : ''
    });
  }

  formatDateForInput(date: Date): string {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    const hours = String(date.getHours()).padStart(2, '0');
    const minutes = String(date.getMinutes()).padStart(2, '0');
    return `${year}-${month}-${day}T${hours}:${minutes}`;
  }

  onSubmit(): void {
    if (this.taskForm.invalid) {
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';

    const formValue = this.taskForm.value;
    // const taskData = {
    //   title: formValue.title,
    //   description: formValue.description || undefined,
    //   priority: formValue.priority,
    //   dueDate: formValue.dueDate ? new Date(formValue.dueDate).toISOString() : undefined
    // };
    const taskData: any = {
      title: formValue.title,
      priority: Number(formValue.priority) // Ensure priority is a number
    };

    if (formValue.description && formValue.description.trim()) {
      taskData.description = formValue.description;
    }

    if (formValue.dueDate) {
      taskData.dueDate = new Date(formValue.dueDate).toISOString();
    }

    if (this.isEditMode && this.task?.id) {
      const updateData: UpdateTaskRequest = {
        ...taskData,
        isCompleted: this.task.isCompleted
      };

      this.taskService.updateTask(this.task.id, updateData).subscribe({
        next: () => {
          this.taskSaved.emit();
        },
        error: (error) => {
          this.errorMessage = error.error?.message || 'Failed to update task';
          this.isLoading = false;
        }
      });
    } else {
      const createData: CreateTaskRequest = taskData;

      this.taskService.createTask(createData).subscribe({
        next: () => {
          this.taskSaved.emit();
        },
        error: (error) => {
          this.errorMessage = error.error?.message || 'Failed to create task';
          this.isLoading = false;
        }
      });
    }
  }

  onClose(): void {
    this.close.emit();
  }
}