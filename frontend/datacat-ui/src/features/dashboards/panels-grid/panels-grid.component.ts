import { Component, Input } from '@angular/core';
import { DashboardVariable, Panel } from '../../../entities';
import { ApiService } from '../../../shared/services/datacat-generated-client';
import { ToastLoggerService } from '../../../shared/services/toast-logger.service';
import { ButtonModule } from 'primeng/button';
import { SelectModule } from 'primeng/select';
import { InputGroupModule } from 'primeng/inputgroup';
import { InputGroupAddonModule } from 'primeng/inputgroupaddon';
import { SelectButtonModule } from 'primeng/selectbutton';
import { timer } from 'rxjs';

@Component({
  standalone: true,
  selector: 'datacat-panels-grid',
  templateUrl: './panels-grid.component.html',
  styleUrl: './panels-grid.component.scss',
  imports: [
    ButtonModule,
    SelectModule,
    InputGroupModule,
    InputGroupAddonModule,
    SelectButtonModule,
  ],
})
export class PanelsGridComponent {
  constructor(
    private apiService: ApiService,
    private loggerService: ToastLoggerService,
  ) {}

  protected _dashboardId?: string;

  @Input() public set dashboardId(id: string) {
    this._dashboardId = this.dashboardId;
  }

  protected isBusy = false;

  protected variables: DashboardVariable[] = [];

  protected panels: Panel[] = [];

  protected refreshDashboardVariables() {
    if (!this._dashboardId) return;

    this.apiService.getApiV1VariablesDashboard(this._dashboardId).subscribe({
      next: (data) => {
        this.variables = data.map<DashboardVariable>((item) => {
          return {
            id: item.id || '',
            placeholder: item.placeholder || '',
            value: item.value || '',
          };
        });
      },
      error: (e) => {
        this.loggerService.error(e);
      },
    });
  }

  protected refreshDashboardsData() {
    this.isBusy = true;

    timer(2000).subscribe(() => (this.isBusy = false));
  }
}
