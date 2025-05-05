import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import * as urls from '../../../shared/common/urls';
import { DialogModule } from 'primeng/dialog';
import { ApiService } from '../../../shared/services/api.service';
import { timer } from 'rxjs';

@Component({
  standalone: true,
  selector: 'datacat-delete-notification-template-button',
  templateUrl: './delete-notification-template-button.component.html',
  styleUrl: './delete-notification-template-button.component.scss',
  imports: [ButtonModule, DialogModule],
})
export class DeleteNotificationTemplateButtonComponent {
  protected isDeletionInitiated = false;
  protected isDeletionDialogVisible = false;
  protected isDeletionError = false;

  @Input() templateId?: string;

  constructor(
    private apiService: ApiService,
    private router: Router,
  ) {}

  protected showDeletionDialog() {
    this.isDeletionError = false;
    this.isDeletionDialogVisible = true;
  }

  protected hideDeletionDialog() {
    this.isDeletionDialogVisible = false;
  }

  protected deleteNotificationTemplate() {
    this.isDeletionError = false;
    this.isDeletionInitiated = true;
    if (this.templateId) {
      // TODO: call API
      timer(1000).subscribe({
        next: () => this.router.navigateByUrl(urls.NOTIFICATION_CHANNELS_URL),
        error: () => {
          // TODO
          this.isDeletionInitiated = false;
          this.isDeletionError = true;
        },
      });
    }
  }
}
