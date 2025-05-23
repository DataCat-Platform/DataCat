import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import * as urls from '../../../shared/common/urls';
import { DialogModule } from 'primeng/dialog';
import { ApiService } from '../../../shared/services/datacat-generated-client';
import { ToastLoggerService } from '../../../shared/services/toast-logger.service';

@Component({
  standalone: true,
  selector: 'datacat-delete-alert-button',
  templateUrl: './delete-alert-button.component.html',
  styleUrl: './delete-alert-button.component.scss',
  imports: [ButtonModule, DialogModule],
})
export class DeleteAlertButtonComponent {
  @Input() alertId?: string;
  protected isDeletionInitiated = false;
  protected isDeletionDialogVisible = false;
  protected isDeletionError = false;

  constructor(
    private apiService: ApiService,
    private loggerService: ToastLoggerService,
    private router: Router,
  ) {}

  protected showDeletionDialog() {
    this.isDeletionError = false;
    this.isDeletionDialogVisible = true;
  }

  protected hideDeletionDialog() {
    this.isDeletionDialogVisible = false;
  }

  protected deleteAlert() {
    this.isDeletionError = false;
    this.isDeletionInitiated = true;
    if (this.alertId) {
      this.apiService.deleteApiV1AlertRemove(this.alertId).subscribe({
        next: () => {
          this.loggerService.success('Deleted alert');
          this.router.navigateByUrl(urls.ALERTS_EXPLORER_URL);
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
