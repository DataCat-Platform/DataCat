import { Component, Input } from '@angular/core';
import { PanelModule } from 'primeng/panel';
import { ButtonModule } from 'primeng/button';
import { PanelsGridComponent } from '../../features/dashboards/panels-grid/panels-grid.component';

@Component({
  standalone: true,
  selector: 'datacat-view-dashboard',
  templateUrl: './view-dashboard.component.html',
  styleUrl: './view-dashboard.component.scss',
  imports: [PanelModule, ButtonModule, PanelsGridComponent],
})
export class ViewDashboardComponent {
  @Input() protected dashboardId: string = '';
}
