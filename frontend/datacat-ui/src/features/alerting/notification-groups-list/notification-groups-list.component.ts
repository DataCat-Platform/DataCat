import { Component } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { InputTextModule } from 'primeng/inputtext';
import { DataViewModule } from 'primeng/dataview';
import { NotificationGroupExpanded } from '../../../entities';
import { from } from 'rxjs';
import { getFakeNotifcationGroupExpanded } from '../../../shared/mock/fakes';
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
    NotificationGroupComponent,
  ],
})
export class NotificationGroupsListComponent {
  protected groupName = new FormControl<string>('');

  protected notificationGroups?: NotificationGroupExpanded[];

  constructor() {
    this.groupName.valueChanges.subscribe(() => {
      this.refreshNotificationGroups();
    });
    this.refreshNotificationGroups();
  }

  private refreshNotificationGroups() {
    from([getFakeNotifcationGroupExpanded()]).subscribe({
      next: (notificationGroups) => {
        this.notificationGroups = notificationGroups;
      },
      error: () => {
        // TODO
      },
    });
  }
}
