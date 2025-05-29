import { Component, Input } from '@angular/core';
import { PanelModule } from 'primeng/panel';
import { Panel } from '../../../../entities';
import { ButtonModule } from 'primeng/button';
import { Router } from '@angular/router';
import * as urls from '../../../../shared/common/urls';
import { DialogModule } from 'primeng/dialog';
import { TextareaModule } from 'primeng/textarea';
import { DividerModule } from 'primeng/divider';
import { DashboardService } from '../dashboard.service';
import { PanelChartComponent } from '../../../../shared/ui/charts/panel-chart/panel-chart.component';
import { ChartService } from '../../../../shared/ui/charts/chart.service';
import { TimeSeries } from '../../../../shared/ui/charts/chart.types';

@Component({
  standalone: true,
  selector: 'datacat-panel-in-grid',
  templateUrl: './panel-in-grid.component.html',
  styleUrl: './panel-in-grid.component.scss',
  imports: [
    PanelModule,
    PanelChartComponent,
    ButtonModule,
    DialogModule,
    TextareaModule,
    DividerModule,
  ],
  providers: [ChartService],
})
export class PanelInGridComponent {
  @Input() set panel(p: Panel | undefined) {
    this._panel = p;
    if (p) {
      this.chartService.setType(p.visualizationType!);
      this.chartService.setQuery(p.query);
      this.chartService.setDataSourceName(p.dataSource!.name);
      this.chartService.updateStyle(p.visualizationSettings);
    }
    if (this.dashboardService.timeRange) {
      const tr = this.dashboardService.timeRange;
      this.chartService.loadTimeRange(tr.from, tr.to, tr.step);
    }
  }

  protected _panel?: Panel;
  protected isError: boolean = false;
  protected data: TimeSeries[] | null = null;
  protected isDialogShown = false;

  constructor(
    private router: Router,
    private dashboardService: DashboardService,
    private chartService: ChartService,
  ) {
    this.chartService.data$.subscribe((v) => (this.data = v));
    this.chartService.error$.subscribe((v) => (this.isError = v));
    this.dashboardService.timeRange$.subscribe((tr) => {
      if (tr) this.chartService.loadTimeRange(tr.from, tr.to, tr.step);
    });
  }

  protected editPanel() {
    if (this._panel?.id) {
      this.router.navigateByUrl(urls.panelEditUrl(this._panel?.id));
    }
  }

  public showDialog() {
    this.isDialogShown = true;
  }
}
