import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { ApiService } from '../../../shared/services/datacat-generated-client';
import { ToastLoggerService } from '../../../shared/services/toast-logger.service';
import { finalize } from 'rxjs';

@Component({
  standalone: true,
  selector: 'datacat-delete-variable-button',
  templateUrl: './delete-variable-button.component.html',
  styleUrl: './delete-variable-button.component.scss',
  imports: [ButtonModule, DialogModule],
})
export class DeleteVariableButtonComponent {
  @Output() onDelete = new EventEmitter<void>();

  @Input() variableId?: string;
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

  protected deleteVariable() {
    this.isDeletionError = false;
    this.isDeletionInitiated = true;
    if (this.variableId) {
      this.apiService
        .deleteApiV1VariableRemove(this.variableId)
        .pipe(
          finalize(() => {
            this.isDeletionInitiated = false;
          }),
        )
        .subscribe({
          next: () => {
            this.loggerService.success('Deleted variable');
            this.onDelete.emit();
            this.hideDeletionDialog();
          },
          error: (e) => {
            this.loggerService.error(e);
            this.isDeletionError = true;
          },
        });
    }
  }
}
