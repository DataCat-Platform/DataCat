import { Component, Input } from '@angular/core';
import { PanelModule } from 'primeng/panel';
import { NotificationChannel } from '../../../features/alerting/notifications-explorer/notifications-explorer.types';
import { WebhookSettings } from '../../../entities/notifications.entities';
import { ButtonModule } from 'primeng/button';
import { FormsModule } from '@angular/forms';
import { DividerModule } from 'primeng/divider';

@Component({
  standalone: true,
  selector: 'datacat-webhook-channel',
  templateUrl: './webhook-channel.component.html',
  styleUrl: './webhook-channel.component.scss',
  imports: [PanelModule, ButtonModule, FormsModule, DividerModule],
})
export class WebhookChannelComponent {
  @Input() public channel?: NotificationChannel;

  protected settingsAsWebhookSettings(): WebhookSettings {
    return this.channel?.channelSettings as WebhookSettings;
  }
}
