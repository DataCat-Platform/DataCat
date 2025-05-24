import { Component, Input } from '@angular/core';
import { PanelModule } from 'primeng/panel';
import { PanelVisualizationComponent } from '../../../../shared/ui/panel-visualization/panel-visualization.component';
import { Panel } from '../../../../entities';
import { ButtonModule } from 'primeng/button';
import { Router } from '@angular/router';
import * as urls from '../../../../shared/common/urls';
import { TimeSeries } from '../../../../entities/dashboards/data.types';
import { DialogModule } from 'primeng/dialog';
import { TextareaModule } from 'primeng/textarea';
import { DividerModule } from 'primeng/divider';
import { PanelDataService } from '../panel-data.service';
import { DashboardService } from '../dashboard.service';
import { ThemeProvider } from 'primeng/config';

@Component({
  standalone: true,
  selector: 'datacat-panel-in-grid',
  templateUrl: './panel-in-grid.component.html',
  styleUrl: './panel-in-grid.component.scss',
  imports: [
    PanelModule,
    PanelVisualizationComponent,
    ButtonModule,
    DialogModule,
    TextareaModule,
    DividerModule,
  ],
})
export class PanelInGridComponent {
  @Input() set panel(p: Panel | undefined) {
    this._panel = p;
    this.panelDataService.panel = p;
    console.log('wtf');
    if (this.dashboardService.timeRange) {
      console.log('ww');
      this.panelDataService.loadTimeRange(this.dashboardService.timeRange);
    }
  }

  protected _panel?: Panel;
  protected isError: boolean = false;
  protected data: TimeSeries[] | null = null;
  protected isDialogShown = false;

  constructor(
    private router: Router,
    private panelDataService: PanelDataService,
    private dashboardService: DashboardService,
  ) {
    this.panelDataService.data$.subscribe((v) => (this.data = v));
    this.panelDataService.error$.subscribe((v) => (this.isError = v));
    this.dashboardService.timeRange$.subscribe((tr) => {
      if (tr) this.panelDataService.loadTimeRange(tr);
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
