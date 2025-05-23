import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { ApiService } from '../../../shared/services/datacat-generated-client';
import { ToastLoggerService } from '../../../shared/services/toast-logger.service';

@Component({
  standalone: true,
  selector: 'datacat-delete-dashboard-button',
  templateUrl: './delete-dashboard-button.component.html',
  styleUrl: './delete-dashboard-button.component.scss',
  imports: [ButtonModule, DialogModule],
})
export class DeleteDashboardButtonComponent {
  @Output() onDelete = new EventEmitter<void>();

  @Input() dashboardId?: string;
  protected isDeletionInitiated = false;
  protected isDeletionDialogVisible = false;
  protected isDeletionError = false;

  constructor(
    private apiService: ApiService,
    private loggerService: ToastLoggerService,
  ) {}

  protected showDeletionDialog() {
    this.isDeletionError = false;
    this.isDeletionDialogVisible = true;
  }

  protected hideDeletionDialog() {
    this.isDeletionDialogVisible = false;
  }

  protected deleteDashboard() {
    this.isDeletionError = false;
    this.isDeletionInitiated = true;
    if (this.dashboardId) {
      this.apiService.deleteApiV1DashboardRemove(this.dashboardId).subscribe({
        next: () => {
          this.loggerService.success('Deleted dashboard');
          this.onDelete.emit();
        },
        error: (e) => {
          this.loggerService.error(e);
          this.isDeletionInitiated = false;
          this.isDeletionError = true;
        },
      });
    }
  }
}
