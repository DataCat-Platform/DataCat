import { Component, Input } from '@angular/core';
import { PanelModule } from 'primeng/panel';
import { DeleteAlertButtonComponent } from '../../features/alerting/delete-alert/delete-alert-button.component';
import { EditAlertFormComponent } from '../../features/alerting/edit-alert/edit-alert-form/edit-alert-form.component';
import { DividerModule } from 'primeng/divider';

@Component({
  standalone: true,
  selector: 'datacat-manage-notification-group',
  templateUrl: './manage-notification-group.component.html',
  styleUrl: './manage-notification-group.component.scss',
  imports: [
    PanelModule,
    EditAlertFormComponent,
    DeleteAlertButtonComponent,
    DividerModule,
  ],
})
export class ManageNotificationGroupComponent {
  @Input() notificationGroupId: string = '';
}
