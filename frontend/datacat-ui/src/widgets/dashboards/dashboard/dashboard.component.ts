import { Component } from '@angular/core';
import { PanelModule } from 'primeng/panel';
import { Panel } from '../../../entities/dashboard.entities';
import { from, Observable } from 'rxjs';
import { DashboardViewService } from '../../../features/dashboards/dashboard-view.service';
import { SkeletonModule } from 'primeng/skeleton';

@Component({
  standalone: true,
  selector: 'datacat-dashboard-view',
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss',
  imports: [PanelModule, SkeletonModule],
})
export class DashboardComponent {
  public panels: Observable<Panel[]>;

  constructor(private dashboardViewService: DashboardViewService) {
    this.panels = from([]);
  }
}
