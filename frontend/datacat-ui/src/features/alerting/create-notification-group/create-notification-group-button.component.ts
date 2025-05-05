import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import * as urls from '../../../shared/common/urls';
import { ApiService } from '../../../shared/services/api.service';
import { timer } from 'rxjs';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { FormControl, FormsModule, ReactiveFormsModule } from '@angular/forms';

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
  ) {}

  protected createNotificationGroup() {
    // TODO: call API
    this.groupName.disable();
    this.isCreationInitiated = true;
    timer(1000).subscribe({
      next: () => {
        const groupId = '0';
        this.router.navigateByUrl(urls.notificationGroupEditUrl(groupId));
      },
      error: () => {
        // TODO
        this.groupName.enable();
        this.isCreationInitiated = false;
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
