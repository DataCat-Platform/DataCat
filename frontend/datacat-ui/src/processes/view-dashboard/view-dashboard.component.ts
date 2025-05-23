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

  protected dashboard?: Dashboard;

  constructor(
    private apiService: ApiService,
    private loggerService: ToastLoggerService,
    private router: Router,
  ) {}

  ngAfterContentInit() {
    this.refresh();
  }

  protected refresh() {
    this.apiService.getApiV1Dashboard(this.dashboardId).subscribe({
      next: (data) => {
        this.dashboard = {
          id: data.id!,
          name: data.name!,
          description: data.description!,
          panels: [],
          createdAt: data.createdAt!,
          lastUpdatedAt: data.updatedAt!,
        };
        this.editDashboardButtonComponent!.fillForm(
          this.dashboard.name,
          this.dashboard.description,
        );
      },
      error: (e) => {
        this.loggerService.error(e);
      },
    });
  }

  protected showDashboardsList() {
    this.router.navigateByUrl(urls.DASHBOARDS_EXPLORER_URL);
  }
}
