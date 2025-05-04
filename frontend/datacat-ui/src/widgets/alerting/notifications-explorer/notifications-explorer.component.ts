import { Component, OnInit } from '@angular/core';
import { Panel } from 'primeng/panel';
import { TabsModule } from 'primeng/tabs';
import {
  NotificationChannelDriver,
  NotificationGroup,
  NotificationTemplate,
} from '../../../entities';
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
  protected NotificationChannelDriver = NotificationChannelDriver;

  protected channelsGroups?: NotificationGroup[];
  protected notificationTemplates?: NotificationTemplate[];

  constructor() {}

  ngOnInit() {}
}
