import { Component, Input } from '@angular/core';
import { PanelModule } from 'primeng/panel';
import { EmailSettings, NotificationChannel } from '../../../../entities';
import { ButtonModule } from 'primeng/button';
import { FormsModule } from '@angular/forms';

@Component({
  standalone: true,
  selector: 'datacat-email-channel',
  templateUrl: './email-channel.component.html',
  styleUrl: './email-channel.component.scss',
  imports: [PanelModule, ButtonModule, FormsModule],
})
export class EmailChannelComponent {
  @Input() public channel?: NotificationChannel;

  protected settingsAsEmailSettings(): EmailSettings {
    return this.channel?.settings as EmailSettings;
  }
}
