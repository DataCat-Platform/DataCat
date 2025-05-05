import { Component, Input } from '@angular/core';
import { NotificationTemplate } from '../../../../entities';
import { PanelModule } from 'primeng/panel';
import { ButtonModule } from 'primeng/button';

@Component({
  standalone: true,
  selector: 'datacat-notification-template',
  templateUrl: './notification-template.component.html',
  styleUrl: './notification-template.component.scss',
  imports: [PanelModule, ButtonModule],
})
export class NotificationTemplateComponent {
  @Input() public notificationTemplate?: NotificationTemplate;
}
