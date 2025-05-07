import { Component, Input } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import {
  NotificationChannel,
  NotificationChannelDriver,
} from '../../../entities';
import { SelectModule } from 'primeng/select';
import {
  ApiService,
  IAddNotificationChannelRequest,
  NotificationChannelGroupResponse,
} from '../../../shared/services/datacat-generated-client';
import { ToastLoggerService } from '../../../shared/services/toast-logger.service';
import { TextareaModule } from 'primeng/textarea';
import { LoadingState } from '../../../shared/common/enums';
import { TagModule } from 'primeng/tag';
import { SkeletonModule } from 'primeng/skeleton';
import { DialogModule } from 'primeng/dialog';
import { finalize } from 'rxjs';
import { InputNumberModule } from 'primeng/inputnumber';

@Component({
  standalone: true,
  selector: 'datacat-edit-notification-group-form',
  templateUrl: './edit-notification-group-form.component.html',
  styleUrl: './edit-notification-group-form.component.scss',
  imports: [
    ReactiveFormsModule,
    InputTextModule,
    ButtonModule,
    SelectModule,
    TextareaModule,
    TagModule,
    SkeletonModule,
    DialogModule,
    InputNumberModule,
  ],
})
export class EditNotificationGroupFormComponent {
  @Input() public set groupId(id: string) {
    if (id) {
      this.loadEssentials(id);
    }
  }

  protected isChannelCreationDialogVisible = false;
  protected groupName: string = '';

  protected NotificationChannelDriver = NotificationChannelDriver;

  protected LoadingState = LoadingState;
  protected loadingState = LoadingState.Loading;

  protected drivers = Object.values(NotificationChannelDriver);

  protected isSavingInitiated = false;

  protected notificationChannels: NotificationChannel[] = [];

  protected editChannelForm = new FormGroup({});

  protected addChannelForm = new FormGroup({
    driver: new FormControl<NotificationChannelDriver>(
      NotificationChannelDriver.EMAIL,
    ),
    settings: new FormGroup<any>({}),
  });

  protected get addChannelFormDriver() {
    return (
      this.addChannelForm.get('driver')?.value ||
      NotificationChannelDriver.EMAIL
    );
  }

  constructor(
    private apiService: ApiService,
    private loggerService: ToastLoggerService,
  ) {
    this.switchAddChannelFormSettingsDriver(NotificationChannelDriver.EMAIL);
  }

  protected loadEssentials(groupId: string) {
    this.apiService.getApiV1NotificationChannelGroup(groupId).subscribe({
      next: (group) => {
        this.groupName = group.name || '';
        this.parseNotificationGroupChannels(group);
        this.loadingState = LoadingState.Success;
      },
      error: (e) => {
        this.loggerService.error(e);
        this.loadingState = LoadingState.Error;
      },
    });
  }

  protected switchAddChannelFormSettingsDriver(
    driver: NotificationChannelDriver,
  ) {
    switch (driver) {
      case NotificationChannelDriver.EMAIL: {
        this.addChannelForm.setControl(
          'settings',
          new FormGroup({
            DestinationEmail: new FormControl<string>(''),
            SmtpServer: new FormControl<string>(''),
            Port: new FormControl<number>(80),
            PasswordPath: new FormControl<string>(''),
          }),
        );
        break;
      }
      case NotificationChannelDriver.TELEGRAM: {
        this.addChannelForm.setControl(
          'settings',
          new FormGroup({
            TelegramTokenPath: new FormControl<string>(''),
            PasswordPath: new FormControl<string>(''),
          }),
        );
        break;
      }
      case NotificationChannelDriver.WEBHOOK: {
        this.addChannelForm.setControl(
          'settings',
          new FormGroup({
            Url: new FormControl<string>(''),
          }),
        );
        break;
      }
    }
  }

  protected parseNotificationGroupChannels(
    group: NotificationChannelGroupResponse,
  ) {
    this.notificationChannels = [];
  }

  protected showChannelCreationDialog() {
    this.isChannelCreationDialogVisible = true;
  }

  protected hideChannelCreationDialog() {
    this.isChannelCreationDialogVisible = false;
  }

  protected addChannel() {
    this.addChannelForm.disable();

    const request: any = {
      notificationChannelGroupName: this.groupName,
      destinationName: this.addChannelForm.get('driver')?.value,
      settings: JSON.stringify(this.addChannelForm.get('settings')?.value),
    } as IAddNotificationChannelRequest;

    console.log(request);

    this.apiService
      .postApiV1NotificationChannelAdd(request)
      .pipe(
        finalize(() => {
          this.addChannelForm.enable();
        }),
      )
      .subscribe({
        next: () => {
          this.loadEssentials(this.groupId);
        },
        error: (e) => {
          this.loggerService.error(e);
        },
      });
  }

  protected deleteChannel(chanel: NotificationChannel) {}
}
