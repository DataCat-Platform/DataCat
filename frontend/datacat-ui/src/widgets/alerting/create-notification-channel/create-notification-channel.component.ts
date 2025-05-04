import { Component } from '@angular/core';
import { SelectModule } from 'primeng/select';
import { EmailSettings, NotificationChannelDriver } from '../../../entities';
import { EmailSettingsComponent } from '../email-settings/email-settings.component';

@Component({
  standalone: true,
  selector: 'datacat-create-notification-channel',
  templateUrl: './create-notification-channel.component.html',
  imports: [SelectModule, EmailSettingsComponent],
})
export class CreateNotificationChannelComponent {
  protected NotificationChannelDriver = NotificationChannelDriver;
  protected notificationChannelType: NotificationChannelDriver =
    NotificationChannelDriver.EMAIL;

  protected setSettings(settings: EmailSettings) {}
}
