import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import * as urls from '../../../shared/common/urls';
import { finalize, timer } from 'rxjs';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { FormControl, FormsModule, ReactiveFormsModule } from '@angular/forms';
import {
  ApiService,
  IAddNotificationChannelGroupRequest,
} from '../../../shared/services/datacat-generated-client';
import { ToastLoggerService } from '../../../shared/services/toast-logger.service';

@Component({
  standalone: true,
  selector: './datacat-create-notification-group-button',
  templateUrl: './create-notification-group-button.component.html',
  styleUrl: './create-notification-group-button.component.scss',
  imports: [
    ButtonModule,
    InputTextModule,
    DialogModule,
    FormsModule,
    ReactiveFormsModule,
  ],
})
export class CreateNotificationGroupButtonComponent {
  protected isCreationInitiated = false;
  protected isDialogVisible = false;

  protected groupName = new FormControl<string>('');

  constructor(
    private router: Router,
    private apiService: ApiService,
    private loggerService: ToastLoggerService,
  ) {}

  protected createNotificationGroup() {
    this.groupName.disable();
    this.isCreationInitiated = true;
    const request: any = {
      name: this.groupName.value,
    } as IAddNotificationChannelGroupRequest;
    this.apiService
      .postApiV1NotificationChannelGroupAdd(request)
      .pipe(
        finalize(() => {
          this.groupName.enable();
          this.isCreationInitiated = false;
        }),
      )
      .subscribe({
        next: (groupId: string) => {
          this.router.navigateByUrl(urls.notificationGroupEditUrl(groupId));
          this.loggerService.success('Successfully created notification group');
        },
        error: (e) => {
          this.loggerService.error(e);
        },
      });
  }

  protected showCreationDialog() {
    this.isDialogVisible = true;
  }

  protected hideCreationDialog() {
    this.isDialogVisible = false;
  }
}
