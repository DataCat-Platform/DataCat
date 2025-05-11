import { Component, Input } from '@angular/core';
import { EmailChannelComponent } from '../email-channel/email-channel.component';
import { WebhookChannelComponent } from '../webhook-channel/webhook-channel.component';
import { TelegramChannelComponent } from '../telegram-channel/telegram-channel.component';
import {
  NotificationChannelDriver,
  NotificationGroupExpanded,
} from '../../../../entities';
import { PanelModule } from 'primeng/panel';
import { ButtonModule } from 'primeng/button';
import { Router } from '@angular/router';
import * as urls from '../../../../shared/common/urls';

@Component({
  standalone: true,
  selector: 'datacat-notification-group',
  templateUrl: './notification-group.component.html',
  styleUrl: './notification-group.component.scss',
  imports: [
    EmailChannelComponent,
    WebhookChannelComponent,
    TelegramChannelComponent,
    PanelModule,
    ButtonModule,
  ],
})
export class NotificationGroupComponent {
  protected NotificationChannelDriver = NotificationChannelDriver;

  @Input() public notificationGroup?: NotificationGroupExpanded;

  constructor(private router: Router) {}

  protected editNotificationGroup() {
    if (this.notificationGroup) {
      this.router.navigateByUrl(
        urls.notificationGroupEditUrl(this.notificationGroup.id),
      );
    }
  }
}
