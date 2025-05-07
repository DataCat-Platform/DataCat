import { Component } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { InputTextModule } from 'primeng/inputtext';
import { DataViewModule } from 'primeng/dataview';
import {
  NotificationChannel,
  NotificationChannelDriver,
  NotificationGroupExpanded,
} from '../../../entities';
import {
  ApiService,
  NotificationChannelResponse,
} from '../../../shared/services/datacat-generated-client';
import { ToastLoggerService } from '../../../shared/services/toast-logger.service';
import { ButtonModule } from 'primeng/button';
import { ListboxModule } from 'primeng/listbox';
import { NotificationGroupComponent } from './notification-group/notification-group.component';

@Component({
  standalone: true,
  selector: 'datacat-notification-groups-list',
  templateUrl: './notification-groups-list.component.html',
  styleUrl: './notification-groups-list.component.scss',
  imports: [
    InputTextModule,
    ReactiveFormsModule,
    DataViewModule,
    ButtonModule,
    ListboxModule,
    NotificationGroupComponent,
  ],
})
export class NotificationGroupsListComponent {
  protected isRefreshing = false;

  protected groupNameControl = new FormControl<string>('');

  protected filteredNotificationGroups: NotificationGroupExpanded[] = [];
  protected notificationGroups: NotificationGroupExpanded[] = [];

  constructor(
    private apiService: ApiService,
    private loggerService: ToastLoggerService,
  ) {
    this.refreshNotificationGroups();
    this.groupNameControl.valueChanges.subscribe((value) =>
      this.updateFilteredNotificationGroups(value),
    );
  }

  private updateFilteredNotificationGroups(groupName: string | null) {
    this.filteredNotificationGroups = this.notificationGroups.filter((group) =>
      group.name.includes(groupName || ''),
    );
  }

  private refreshNotificationGroups() {
    this.apiService.getApiV1NotificationChannelGroupGetAll().subscribe({
      next: (notificationGroups) => {
        this.notificationGroups =
          notificationGroups.map<NotificationGroupExpanded>((group) => {
            return {
              id: group.id || '',
              name: group.name || '',
              notificationChannels: (
                group.notificationChannels || []
              ).map<NotificationChannel>(
                (channel: NotificationChannelResponse) => {
                  return {
                    id: channel.id || 0,
                    driver: NotificationChannelDriver.EMAIL,
                    settings: JSON.parse(channel.settings || ''),
                  };
                },
              ),
            };
          });
        this.updateFilteredNotificationGroups(this.groupNameControl.value);
      },
      error: (e) => {
        this.loggerService.error(e);
      },
    });
  }
}
