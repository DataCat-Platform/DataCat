import { Component, Input } from '@angular/core';
import { EmailChannelComponent } from '../email-channel/email-channel.component';
import { WebhookChannelComponent } from '../webhook-channel/webhook-channel.component';
import { TelegramChannelComponent } from '../telegram-channel/telegram-channel.component';
import { NotificationChannelDriver, NotificationGroup } from '../../../entities';
import { PanelModule } from 'primeng/panel';
import { ButtonModule } from 'primeng/button';

@Component({
  standalone: true,
  selector: 'datacat-channels-group',
  templateUrl: './channels-group.component.html',
  styleUrl: './channels-group.component.scss',
  imports: [
    EmailChannelComponent,
    WebhookChannelComponent,
    TelegramChannelComponent,
    PanelModule,
    ButtonModule,
  ],
})
export class ChannelsGroupComponent {
  protected NotificationChannelDriver = NotificationChannelDriver;

  @Input() public channelsGroup?: NotificationGroup;
}
