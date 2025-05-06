import { Component, Input } from '@angular/core';
import { DividerModule } from 'primeng/divider';
import { TabsModule } from 'primeng/tabs';
import {
  Alert,
  DataSource,
  NotificationGroup,
} from '../../../entities/alerting';
import { from } from 'rxjs';
import { CommonModule } from '@angular/common';
import { InputGroupModule } from 'primeng/inputgroup';
import { InputGroupAddonModule } from 'primeng/inputgroupaddon';
import { TagModule } from 'primeng/tag';
import { TooltipModule } from 'primeng/tooltip';
import {
  FAKE_ALERT,
  FAKE_DATASOURCE,
} from '../../../shared/mock/fakes';

@Component({
  standalone: true,
  selector: 'datacat-alert-details',
  templateUrl: './alert-details.component.html',
  styleUrl: './alert-details.component.scss',
  imports: [
    DividerModule,
    TabsModule,
    CommonModule,
    InputGroupModule,
    InputGroupAddonModule,
    TagModule,
    TooltipModule,
  ],
})
export class AlertDetailsComponent {
  @Input() set alertId(id: string) {
    this.loadEssentials(id);
  }

  protected alert?: Alert;
  protected dataSource?: DataSource;
  protected notificationGroup?: NotificationGroup;

  protected loadEssentials(id: string) {
    // TODO: call API
    from([FAKE_ALERT]).subscribe({
      next: (alert) => (this.alert = alert),
      error: () => {
        // TODO
      },
    });

    from([FAKE_DATASOURCE]).subscribe({
      next: (dataSource) => (this.dataSource = dataSource),
      error: () => {
        // TODO
      },
    });

    // from([FAKE_NOTIFICATION_GROUP]).subscribe({
    //   next: (notificationGroup) => (this.notificationGroup = notificationGroup),
    //   error: () => {
    //     // TODO
    //   },
    // });
  }
}
