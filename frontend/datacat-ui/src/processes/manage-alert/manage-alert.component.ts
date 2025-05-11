import { Component, Input } from '@angular/core';
import { PanelModule } from 'primeng/panel';
import { DeleteAlertButtonComponent } from '../../features/alerting/delete-alert';
import { EditAlertFormComponent } from '../../features/alerting/edit-alert';
import { DividerModule } from 'primeng/divider';
import { MuteAlertButtonComponent } from '../../features/alerting/mute-alert/mute-alert-button.component';

@Component({
  standalone: true,
  selector: 'datacat-edit-alert',
  templateUrl: './manage-alert.component.html',
  styleUrl: './manage-alert.component.scss',
  imports: [
    PanelModule,
    EditAlertFormComponent,
    DeleteAlertButtonComponent,
    DividerModule,
    MuteAlertButtonComponent,
  ],
})
export class ManageAlertComponent {
  @Input() alertId: string = '';
}
