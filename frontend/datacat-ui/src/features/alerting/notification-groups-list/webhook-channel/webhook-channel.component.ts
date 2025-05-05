import { Component, Input } from '@angular/core';
import { PanelModule } from 'primeng/panel';
import { NotificationChannel, WebhookSettings } from '../../../../entities';
import { ButtonModule } from 'primeng/button';
import { FormsModule } from '@angular/forms';

@Component({
  standalone: true,
  selector: 'datacat-webhook-channel',
  templateUrl: './webhook-channel.component.html',
  styleUrl: './webhook-channel.component.scss',
  imports: [PanelModule, ButtonModule, FormsModule],
})
export class WebhookChannelComponent {
  @Input() public channel?: NotificationChannel;

  protected settingsAsWebhookSettings(): WebhookSettings {
    return this.channel?.settings as WebhookSettings;
  }
}
