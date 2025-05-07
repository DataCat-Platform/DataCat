import { Component } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { InputTextModule } from 'primeng/inputtext';
import { DataViewModule } from 'primeng/dataview';
import {
  NotificationChannel,
  NotificationChannelDriver,
  NotificationGroupExpanded,
} from '../../../entities';
import { NotificationGroupComponent } from './notification-group/notification-group.component';
import { ApiService, NotificationChannelResponse } from '../../../shared/services/datacat-generated-client';
import { ToastLoggerService } from '../../../shared/services/toast-logger.service';

@Component({
  standalone: true,
  selector: 'datacat-notification-groups-list',
  templateUrl: './notification-groups-list.component.html',
  styleUrl: './notification-groups-list.component.scss',
  imports: [
    InputTextModule,
    ReactiveFormsModule,
    DataViewModule,
    NotificationGroupComponent,
  ],
})
export class NotificationGroupsListComponent {
  protected groupName = new FormControl<string>('');

  protected notificationGroups?: NotificationGroupExpanded[];

  constructor(
    private apiService: ApiService,
    private loggerService: ToastLoggerService,
  ) {
    this.groupName.valueChanges.subscribe(() => {
      this.refreshNotificationGroups();
    });
    this.refreshNotificationGroups();
  }

  private refreshNotificationGroups() {
    this.apiService.getApiV1NotificationChannelGroupGetAll().subscribe({
      next: (notificationGroups) => {
        console.log(notificationGroups);
        this.notificationGroups =
          notificationGroups.map<NotificationGroupExpanded>((group) => {
            return {
              id: group.id || '',
              name: group.name || '',
              notificationChannels: (
                group.notificationChannels || []
              ).map<NotificationChannel>((channel: NotificationChannelResponse) => {
                return {
                  id: channel.id || 0,
                  driver: NotificationChannelDriver.EMAIL,
                  settings: JSON.parse(channel.settings || ''),
                };
              }),
            };
          });
      },
      error: (e) => {
        this.loggerService.error(e);
      },
    });
  }
}
