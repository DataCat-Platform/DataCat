import { AfterContentInit, Component, Input, ViewChild } from '@angular/core';
import { PanelModule } from 'primeng/panel';
import { ButtonModule } from 'primeng/button';
import { PanelsGridComponent } from '../../features/dashboards/panels-grid/panels-grid.component';
import { DeleteDashboardButtonComponent } from '../../features/dashboards/delete-dashboard';
import { Router } from '@angular/router';
import * as urls from '../../shared/common/urls';
import { EditDashboardButtonComponent } from '../../features/dashboards/edit-dashboard';
import { Dashboard } from '../../entities';
import { ApiService } from '../../shared/services/datacat-generated-client';
import { ToastLoggerService } from '../../shared/services/toast-logger.service';
import { TooltipModule } from 'primeng/tooltip';
import { DashboardService } from '../../features/dashboards/panels-grid/dashboard.service';

@Component({
  standalone: true,
  selector: 'datacat-view-dashboard',
  templateUrl: './view-dashboard.component.html',
  styleUrl: './view-dashboard.component.scss',
  imports: [
    PanelModule,
    ButtonModule,
    PanelsGridComponent,
    DeleteDashboardButtonComponent,
    EditDashboardButtonComponent,
    TooltipModule,
  ],
})
export class ViewDashboardComponent implements AfterContentInit {
  @Input() protected dashboardId: string = '';

  @ViewChild(EditDashboardButtonComponent)
  editDashboardButtonComponent?: EditDashboardButtonComponent;

  protected dashboard: Dashboard | null = null;

  constructor(
    private dashboardService: DashboardService,
    private router: Router,
  ) {
    this.dashboardService.dashboard$.subscribe((v) => (this.dashboard = v));
  }

  ngAfterContentInit() {
    this.dashboardService.dashboardId = this.dashboardId;
  }

  protected refreshDashboardOnly() {
    this.dashboardService.refreshDashboardOnly();
  }

  protected showDashboardsList() {
    this.router.navigateByUrl(urls.DASHBOARDS_EXPLORER_URL);
  }
}
