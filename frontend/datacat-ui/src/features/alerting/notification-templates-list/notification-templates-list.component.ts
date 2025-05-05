import { Component } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { InputTextModule } from 'primeng/inputtext';
import { DataViewModule } from 'primeng/dataview';
import {
  NotificationGroupExpanded,
  NotificationTemplate,
} from '../../../entities';
import { from } from 'rxjs';
import {
  getFakeNotifcationGroupExpanded,
  getFakeNotificationTemplate,
} from '../../../shared/mock/fakes';
import { NotificationTemplateComponent } from './notification-template/notification-template.component';

@Component({
  standalone: true,
  selector: 'datacat-notification-templates-list',
  templateUrl: './notification-templates-list.component.html',
  styleUrl: './notification-templates-list.component.scss',
  imports: [
    InputTextModule,
    ReactiveFormsModule,
    DataViewModule,
    NotificationTemplateComponent,
  ],
})
export class NotificationTemplatesListComponent {
  protected templateName = new FormControl<string>('');

  protected notificationTemplates?: NotificationTemplate[];

  constructor() {
    this.templateName.valueChanges.subscribe(() => {
      this.refreshNotificationTemplates();
    });
    this.refreshNotificationTemplates();
  }

  private refreshNotificationTemplates() {
    const templatesCount = 3;
    const templates = [...Array(templatesCount)].map(() => {
      return getFakeNotificationTemplate();
    });

    from([templates]).subscribe({
      next: (notificationTemplates) => {
        this.notificationTemplates = notificationTemplates;
      },
      error: () => {
        // TODO
      },
    });
  }
}
