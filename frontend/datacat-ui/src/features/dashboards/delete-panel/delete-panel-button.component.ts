import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { ApiService } from '../../../shared/services/datacat-generated-client';
import { ToastLoggerService } from '../../../shared/services/toast-logger.service';

@Component({
  standalone: true,
  selector: 'datacat-delete-panel-button',
  templateUrl: './delete-panel-button.component.html',
  styleUrl: './delete-panel-button.component.scss',
  imports: [ButtonModule, DialogModule],
})
export class DeletePanelButtonComponent {
  @Output() onDelete = new EventEmitter<void>();

  @Input() panelId?: string;
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

  protected deletePanel() {
    this.isDeletionError = false;
    this.isDeletionInitiated = true;
    if (this.panelId) {
      this.apiService.deleteApiV1PanelRemove(this.panelId).subscribe({
        next: () => {
          this.loggerService.success('Deleted panel');
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
