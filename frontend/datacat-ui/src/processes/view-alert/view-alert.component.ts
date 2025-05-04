import { Component } from '@angular/core';
import { PanelModule } from 'primeng/panel';
import { AlertDetailsComponent } from '../../features/alerting/alert-details/alert-details.component';
import { ButtonModule } from 'primeng/button';
import { GoToEditAlertButtonComponent } from '../../features/alerting/edit-alert/goto-edit-alert-button/goto-edit-alert-button.component';

@Component({
  standalone: true,
  selector: 'datacat-view-alert',
  templateUrl: './view-alert.component.html',
  styleUrl: './view-alert.component.scss',
  imports: [
    PanelModule,
    ButtonModule,
    AlertDetailsComponent,
    GoToEditAlertButtonComponent,
  ],
})
export class ViewAlertComponent {
  protected alertId: string;

  constructor() {
    // TODO: take from router
    this.alertId = '12';
  }
}
