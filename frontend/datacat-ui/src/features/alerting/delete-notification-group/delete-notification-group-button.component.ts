import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import * as urls from '../../../shared/common/urls';
import { DialogModule } from 'primeng/dialog';
import { ApiService } from '../../../shared/services/api.service';
import { timer } from 'rxjs';

@Component({
  standalone: true,
  selector: 'datacat-delete-notification-group-button',
  templateUrl: './delete-notification-group-button.component.html',
  styleUrl: './delete-notification-group-button.component.scss',
  imports: [ButtonModule, DialogModule],
})
export class DeleteNotificationGroupButtonComponent {
  protected isDeletionInitiated = false;
  protected isDeletionDialogVisible = false;
  protected isDeletionError = false;

  @Input() groupId?: string;

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

  protected deleteNotificationGroup() {
    this.isDeletionError = false;
    this.isDeletionInitiated = true;
    if (this.groupId) {
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
