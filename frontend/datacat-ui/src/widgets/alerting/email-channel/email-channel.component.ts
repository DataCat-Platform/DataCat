import { Component, Input } from '@angular/core';
import { PanelModule } from 'primeng/panel';
import { NotificationChannel } from '../../../features/alerting/notifications-explorer/notifications-explorer.types';
import { EmailSettings } from '../../../entities/notifications.entities';
import { ButtonModule } from 'primeng/button';
import { FormsModule } from '@angular/forms';
import { DividerModule } from 'primeng/divider';

@Component({
  standalone: true,
  selector: 'datacat-email-channel',
  templateUrl: './email-channel.component.html',
  styleUrl: './email-channel.component.scss',
  imports: [PanelModule, ButtonModule, FormsModule, DividerModule],
})
export class EmailChannelComponent {
  @Input() public channel?: NotificationChannel;

  protected settingsAsEmailSettings(): EmailSettings {
    return this.channel?.channelSettings as EmailSettings;
  }
}
