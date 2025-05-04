import { Component, Input } from '@angular/core';
import { PanelModule } from 'primeng/panel';
import { TextareaModule } from 'primeng/textarea';
import { FormsModule } from '@angular/forms';
import { NotificationTemplate } from '../../../entities';

@Component({
  standalone: true,
  selector: 'datacat-notification-template',
  templateUrl: './notification-template.component.html',
  styleUrl: './notification-template.component.scss',
  imports: [PanelModule, TextareaModule, FormsModule],
})
export class NotificationTemplateComponent {
  @Input() notificationTemplate!: NotificationTemplate;
}
