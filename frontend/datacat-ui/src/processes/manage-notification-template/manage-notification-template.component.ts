import { Component, Input } from '@angular/core';
import { PanelModule } from 'primeng/panel';
import { DividerModule } from 'primeng/divider';
import { EditNotificationTemplateFormComponent } from '../../features/alerting/edit-notification-temlplate';
import { DeleteNotificationTemplateButtonComponent } from '../../features/alerting/delete-notification-template';

@Component({
  standalone: true,
  selector: 'datacat-manage-notification-template',
  templateUrl: './manage-notification-template.component.html',
  styleUrl: './manage-notification-template.component.scss',
  imports: [
    PanelModule,
    EditNotificationTemplateFormComponent,
    DeleteNotificationTemplateButtonComponent,
    DividerModule,
  ],
})
export class ManageNotificationTemplateComponent {
  @Input() notificationTemplateId: string = '';
}
