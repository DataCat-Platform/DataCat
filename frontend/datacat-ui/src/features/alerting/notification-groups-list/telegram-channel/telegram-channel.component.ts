import { Component, Input } from '@angular/core';
import { PanelModule } from 'primeng/panel';
import { NotificationChannel, TelegramSettings } from '../../../../entities';
import { ButtonModule } from 'primeng/button';
import { FormsModule } from '@angular/forms';

@Component({
  standalone: true,
  selector: 'datacat-telegram-channel',
  templateUrl: './telegram-channel.component.html',
  styleUrl: './telegram-channel.component.scss',
  imports: [PanelModule, ButtonModule, FormsModule],
})
export class TelegramChannelComponent {
  @Input() public channel?: NotificationChannel;

  protected settingsAsTelegramSettings(): TelegramSettings {
    return this.channel?.settings as TelegramSettings;
  }
}
