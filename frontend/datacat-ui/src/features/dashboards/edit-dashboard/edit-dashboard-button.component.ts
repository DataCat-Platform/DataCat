import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { finalize } from 'rxjs';
import { InputTextModule } from 'primeng/inputtext';
import { TextareaModule } from 'primeng/textarea';
import { PanelModule } from 'primeng/panel';
import { ApiService } from '../../../shared/services/datacat-generated-client';
import { ToastLoggerService } from '../../../shared/services/toast-logger.service';

@Component({
  standalone: true,
  selector: './datacat-edit-dashboard-button',
  templateUrl: './edit-dashboard-button.component.html',
  styleUrl: './edit-dashboard-button.component.scss',
  imports: [
    ButtonModule,
    DialogModule,
    ReactiveFormsModule,
    InputTextModule,
    TextareaModule,
    PanelModule,
  ],
})
export class EditDashboardButtonComponent {
  @Output() onEdit = new EventEmitter<void>();

  @Input() public dashboardId?: string;

  protected isEditDialogVisible = false;
  protected isEditInitiated = false;

  protected editForm = new FormGroup({
    name: new FormControl<string>('', Validators.required),
    description: new FormControl<string>('', Validators.required),
  });

  protected get nameControl() {
    return this.editForm.get('name')!;
  }

  protected get descriptionControl() {
    return this.editForm.get('description')!;
  }

  constructor(
    private apiService: ApiService,
    private loggerService: ToastLoggerService,
  ) {}

  protected editDashboard() {
    this.editForm.markAllAsTouched();
    this.editForm.updateValueAndValidity();

    if (this.editForm.invalid || !this.dashboardId) return;

    this.isEditInitiated = true;

    const request: any = this.editForm.getRawValue();

    this.apiService
      .putApiV1DashboardUpdate(this.dashboardId, request)
      .pipe(
        finalize(() => {
          this.isEditInitiated = false;
        }),
      )
      .subscribe({
        next: () => {
          this.isEditDialogVisible = false;
          this.editForm.reset();
          this.onEdit.emit();
        },
        error: (e) => {
          this.loggerService.error(e);
        },
      });
  }

  public fillForm(name: string, description: string) {
    this.editForm.setValue({
      name,
      description,
    });
  }
}
