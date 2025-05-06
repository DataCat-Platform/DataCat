import { Component, Input } from '@angular/core';
import {
  FormArray,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
} from '@angular/forms';
import { from, timer } from 'rxjs';
import {
  FAKE_NOTIFICATION_GROUP,
  getFakeNotificationChannel,
} from '../../../shared/mock/fakes';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { NotificationChannelDriver } from '../../../entities';
import { SelectModule } from 'primeng/select';
import { InputGroupModule } from 'primeng/inputgroup';

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
    InputGroupModule,
  ],
})
export class EditNotificationGroupFormComponent {
  @Input() public set groupId(id: string) {
    this.loadEssentials(id);
  }

  protected drivers = Object.values(NotificationChannelDriver);

  protected isSavingInitiated = false;

  protected driverControl = new FormControl<NotificationChannelDriver>(
    NotificationChannelDriver.EMAIL,
  );

  protected form = new FormGroup({
    name: new FormControl<string>(''),
    emailChannels: new FormArray<FormGroup>([]),
    webhookChannels: new FormArray<FormGroup>([]),
    telegramChannels: new FormArray<FormGroup>([]),
  });

  protected get emailChannels(): FormArray<FormGroup> {
    return this.form.get('emailChannels') as FormArray<FormGroup>;
  }

  protected get webhookChannels(): FormArray<FormGroup> {
    return this.form.get('webhookChannels') as FormArray<FormGroup>;
  }

  protected get telegramChannels(): FormArray<FormGroup> {
    return this.form.get('telegramChannels') as FormArray<FormGroup>;
  }

  protected loadEssentials(groupId: string) {
    from([FAKE_NOTIFICATION_GROUP]).subscribe({
      next: (group) => {
        this.form.get('name')?.setValue(group.name);
      },
      error: () => {},
    });
    from([
      [getFakeNotificationChannel(), getFakeNotificationChannel()],
    ]).subscribe({
      next: (notificationChannels) => {
        notificationChannels.forEach((channel) => {
          // TODO
        })
      },
      error: () => {},
    });
  }

  protected saveNotificationGroup() {
    this.form.disable();
    this.isSavingInitiated = true;

    timer(1000).subscribe({
      next: () => {
        this.isSavingInitiated = false;
        this.form.enable();
      },
      error: () => {
        this.isSavingInitiated = false;
        this.form.enable();
      },
    });
  }

  protected addChannel() {
    const driver = this.driverControl.value;
    switch (driver) {
      case NotificationChannelDriver.EMAIL: {
        this.emailChannels.push(
          new FormGroup({
            address: new FormControl<string>(''),
          }),
        );
        break;
      }
      case NotificationChannelDriver.TELEGRAM: {
        this.telegramChannels.push(
          new FormGroup({
            username: new FormControl<string>(''),
          }),
        );
        break;
      }
      case NotificationChannelDriver.WEBHOOK: {
        this.webhookChannels.push(
          new FormGroup({
            url: new FormControl<string>(''),
          }),
        );
        break;
      }
    }
  }

  protected deleteEmailChannel(index: number) {
    this.emailChannels.removeAt(index);
  }

  protected deleteWebhookChannel(index: number) {
    this.webhookChannels.removeAt(index);
  }

  protected deleteTelegramChannel(index: number) {
    this.telegramChannels.removeAt(index);
  }
}
