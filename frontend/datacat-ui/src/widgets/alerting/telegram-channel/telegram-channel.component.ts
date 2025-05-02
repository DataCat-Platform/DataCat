import { Component, Input } from '@angular/core';
import { PanelModule } from 'primeng/panel';
import { NotificationChannel } from '../../../features/alerting/notifications-explorer/notifications-explorer.types';
import { TelegramSettings } from '../../../entities/notifications.entities';
import { ButtonModule } from 'primeng/button';
import { FormsModule } from '@angular/forms';
import { DividerModule } from 'primeng/divider';

@Component({
  standalone: true,
  selector: 'datacat-telegram-channel',
  templateUrl: './telegram-channel.component.html',
  styleUrl: './telegram-channel.component.scss',
  imports: [PanelModule, ButtonModule, FormsModule, DividerModule],
})
export class TelegramChannelComponent {
  @Input() public channel?: NotificationChannel;

  protected settingsAsTelegramSettings(): TelegramSettings {
    return this.channel?.channelSettings as TelegramSettings;
  }
}
