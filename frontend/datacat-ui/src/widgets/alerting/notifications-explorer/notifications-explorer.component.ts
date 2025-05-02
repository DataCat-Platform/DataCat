import { Component, OnInit } from '@angular/core';
import { Panel } from 'primeng/panel';
import { TabsModule } from 'primeng/tabs';
import { NotificationChannelType } from '../../../entities';
import { NotificationsExplorerService } from '../../../features/alerting/notifications-explorer/notifications-explorer.service';
import {
  NotificationChannelsGroup,
  NotificationTemplate,
} from '../../../features/alerting/notifications-explorer/notifications-explorer.types';
import { ScrollPanelModule } from 'primeng/scrollpanel';
import { ChannelsGroupComponent } from '../channels-group/channels-group.component';
import { NotificationTemplateComponent } from '../notification-template/notification-template.component';
import { DividerModule } from 'primeng/divider';
import { ButtonModule } from 'primeng/button';

@Component({
  standalone: true,
  selector: './datacat-notification-channels-crud',
  templateUrl: './notifications-explorer.component.html',
  styleUrl: './notifications-explorer.component.scss',
  imports: [
    Panel,
    TabsModule,
    ScrollPanelModule,
    ChannelsGroupComponent,
    NotificationTemplateComponent,
    DividerModule,
    ButtonModule,
  ],
})
export class NotificationChannelsComponent implements OnInit {
  protected NotificationChannelType = NotificationChannelType;

  protected channelsGroups?: NotificationChannelsGroup[];
  protected notificationTemplates?: NotificationTemplate[];

  constructor(
    private notificationsExplorerService: NotificationsExplorerService,
  ) {}

  ngOnInit() {
    this.notificationsExplorerService.refreshNotificationTemplates();
    this.notificationsExplorerService.refreshChannelsGroups();
    this.notificationsExplorerService.channelsGroups$.subscribe((groups) => {
      this.channelsGroups = groups;
    });
    this.notificationsExplorerService.notificationTemplates$.subscribe(
      (templates) => {
        this.notificationTemplates = templates;
      },
    );
  }
}
