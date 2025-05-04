import { Component } from '@angular/core';
import { SelectModule } from 'primeng/select';
import { EmailSettings, NotificationChannelType } from '../../../entities';
import { EmailSettingsComponent } from '../email-settings/email-settings.component';

@Component({
  standalone: true,
  selector: 'datacat-create-notification-channel',
  templateUrl: './create-notification-channel.component.html',
  imports: [SelectModule, EmailSettingsComponent],
})
export class CreateNotificationChannelComponent {
  protected NotificationChannelType = NotificationChannelType;
  protected notificationChannelType: NotificationChannelType =
    NotificationChannelType.EMAIL;

  protected setSettings(settings: EmailSettings) {

  }
}
