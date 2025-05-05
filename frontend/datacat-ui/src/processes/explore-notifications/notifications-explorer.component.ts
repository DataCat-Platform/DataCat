import { Component } from '@angular/core';
import { Panel } from 'primeng/panel';
import { TabsModule } from 'primeng/tabs';
import {
  NotificationChannelDriver,
  NotificationGroup,
  NotificationTemplate,
} from '../../entities';
import { DividerModule } from 'primeng/divider';
import { ButtonModule } from 'primeng/button';
import { CreateNotificationGroupButtonComponent } from '../../features/alerting/create-notification-group';
import { CreateNotificationTemplateButtonComponent } from '../../features/alerting/create-notification-template';
import { NotificationGroupsListComponent } from '../../features/alerting/notification-groups-list/notification-groups-list.component';
import { NotificationTemplatesListComponent } from '../../features/alerting/notification-templates-list';

@Component({
  standalone: true,
  selector: './datacat-notification-channels-crud',
  templateUrl: './notifications-explorer.component.html',
  styleUrl: './notifications-explorer.component.scss',
  imports: [
    Panel,
    TabsModule,
    DividerModule,
    ButtonModule,
    CreateNotificationGroupButtonComponent,
    CreateNotificationTemplateButtonComponent,
    NotificationGroupsListComponent,
    CreateNotificationTemplateButtonComponent,
    NotificationTemplatesListComponent,
  ],
})
export class NotificationsExplorerComponent {
  protected NotificationChannelDriver = NotificationChannelDriver;

  protected channelsGroups?: NotificationGroup[];
  protected notificationTemplates?: NotificationTemplate[];
}
