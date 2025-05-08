import { Component } from '@angular/core';
import { PanelModule } from 'primeng/panel';
import { DashboardsListComponent } from '../../features/dashboards/dashboards-list';
import { CreateDashboardButtonComponent } from '../../features/dashboards/create-dashboard';

@Component({
  standalone: true,
  selector: 'datacat-dashboards-explore',
  templateUrl: './dashboards-explorer.component.html',
  styleUrl: './dashboards-explorer.component.scss',
  imports: [
    PanelModule,
    DashboardsListComponent,
    CreateDashboardButtonComponent,
  ],
})
export class DashboardsExplorerComponent {}
