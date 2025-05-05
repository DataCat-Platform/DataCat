import { Component, Input } from '@angular/core';
import { PanelModule } from 'primeng/panel';
import { DeleteAlertButtonComponent } from '../../features/alerting/delete-alert/delete-alert-button.component';
import { EditAlertFormComponent } from '../../features/alerting/edit-alert/edit-alert-form/edit-alert-form.component';
import { DividerModule } from 'primeng/divider';

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
  ],
})
export class ManageAlertComponent {
  @Input() alertId: string = 'alert-1';
}
