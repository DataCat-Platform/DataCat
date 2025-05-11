import { Component, Input } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import {
  EmailSettings,
  NotificationChannel,
  NotificationChannelDriver,
  NotificationChannelSettings,
  TelegramSettings,
  WebhookSettings,
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
import { CardModule } from 'primeng/card';

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
    CardModule,
  ],
})
export class EditNotificationGroupFormComponent {
  private _groupId?: string;

  @Input() public set groupId(id: string) {
    this._groupId = id;
    this.refresh();
  }

  protected isChannelEditDialogVisible = false;
  protected editedChannel?: NotificationChannel;

  protected isChannelCreationDialogVisible = false;
  protected groupName: string = '';

  protected NotificationChannelDriver = NotificationChannelDriver;

  protected LoadingState = LoadingState;
  protected loadingState = LoadingState.Loading;

  protected drivers = Object.values(NotificationChannelDriver);

  protected isSavingInitiated = false;

  protected notificationChannels: NotificationChannel[] = [];

  protected editChannelForm = new FormGroup<any>({});

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

  protected refresh() {
    if (!this._groupId) {
      return;
    }

    this.apiService.getApiV1NotificationChannelGroup(this._groupId).subscribe({
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
    this.notificationChannels =
      group.notificationChannels?.map<NotificationChannel>((channel) => {
        return {
          id: channel?.id || 0,
          driver: channel.destinationName as NotificationChannelDriver,
          settings: JSON.parse(channel.settings || ''),
        };
      }) || [];
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

    this.apiService
      .postApiV1NotificationChannelAdd(request)
      .pipe(
        finalize(() => {
          this.addChannelForm.enable();
        }),
      )
      .subscribe({
        next: () => {
          this.loggerService.success('Added channel');
          this.isChannelCreationDialogVisible = false;
          this.refresh();
        },
        error: (e) => {
          this.loggerService.error(e);
        },
      });
  }

  protected editChannel(channel: NotificationChannel) {
    switch (channel.driver) {
      case NotificationChannelDriver.EMAIL: {
        const settings = channel.settings as EmailSettings;
        this.editChannelForm = new FormGroup({
          DestinationEmail: new FormControl<string>(settings.DestinationEmail),
          SmtpServer: new FormControl<string>(settings.SmtpServer),
          Port: new FormControl<number>(settings.Port),
          PasswordPath: new FormControl<string>(settings.PasswordPath),
        });
        break;
      }
      case NotificationChannelDriver.TELEGRAM: {
        const settings = channel.settings as TelegramSettings;
        this.editChannelForm = new FormGroup({
          TelegramTokenPath: new FormControl<string>(
            settings.TelegramTokenPath,
          ),
          ChatId: new FormControl<string>(settings.ChatId),
        });
        break;
      }
      case NotificationChannelDriver.WEBHOOK: {
        const settings = channel.settings as WebhookSettings;
        this.editChannelForm = new FormGroup({
          Url: new FormControl<string>(settings.Url),
        });
        break;
      }
    }

    this.editedChannel = channel;
    this.isChannelEditDialogVisible = true;
  }

  protected saveEditedChannel() {
    this.editChannelForm.markAllAsTouched();
    this.editChannelForm.updateValueAndValidity();

    if (!this.editChannelForm.valid || !this.editedChannel) {
      return;
    }

    const request: any = {
      destinationName: this.editedChannel.driver,
      settings: JSON.stringify(this.editChannelForm.value),
    };

    this.editChannelForm.disable();
    this.apiService
      .putApiV1NotificationChannel(this.editedChannel?.id, request)
      .pipe(
        finalize(() => {
          this.editChannelForm.enable();
        }),
      )
      .subscribe({
        next: () => {
          this.isChannelEditDialogVisible = false;
          this.refresh();
        },
        error: (e) => {
          this.loggerService.error(e);
        },
      });
  }

  protected deleteChannel(channel: NotificationChannel) {
    this.apiService.deleteApiV1NotificationChannelRemove(channel.id).subscribe({
      next: () => {
        this.refresh();
      },
      error: (e) => {
        this.loggerService.error(e);
      },
    });
  }

  protected asEmailSettings(
    settings: NotificationChannelSettings,
  ): EmailSettings {
    return settings as EmailSettings;
  }

  protected asWebhookSettings(
    settings: NotificationChannelSettings,
  ): WebhookSettings {
    return settings as WebhookSettings;
  }

  protected asTelegramSettings(
    settings: NotificationChannelSettings,
  ): TelegramSettings {
    return settings as TelegramSettings;
  }
}
