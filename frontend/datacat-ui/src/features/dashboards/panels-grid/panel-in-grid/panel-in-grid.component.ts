import { Component, Input } from '@angular/core';
import { PanelModule } from 'primeng/panel';
import { PanelVisualizationComponent } from '../../../../shared/ui/panel-visualization/panel-visualization.component';
import {
  DataSourceDriver,
  decodeLayout,
  decodeVisualizationSettings,
  encodeLayout,
  encodeVisualizationSettings,
  Layout,
  Panel,
  VisualizationType,
} from '../../../../entities';
import { ApiService } from '../../../../shared/services/datacat-generated-client';
import { ToastLoggerService } from '../../../../shared/services/toast-logger.service';
import { ButtonModule } from 'primeng/button';
import { Router } from '@angular/router';
import * as urls from '../../../../shared/common/urls';
import {
  DataPoint,
  DataPoints,
} from '../../../../entities/dashboards/data.types';
import { DatePipe } from '@angular/common';
import { Observable, of } from 'rxjs';
import { TimeRange } from '../../../../shared/ui/time-range-select';

@Component({
  standalone: true,
  selector: 'datacat-panel-in-grid',
  templateUrl: './panel-in-grid.component.html',
  styleUrl: './panel-in-grid.component.scss',
  imports: [PanelModule, PanelVisualizationComponent, ButtonModule],
})
export class PanelInGridComponent {
  private _panelId?: string;

  @Input() set panelId(id: string | undefined) {
    this._panelId = id;
    this.refresh();
  }

  get panelId() {
    return this._panelId;
  }

  protected data: DataPoints = [];

  protected panel?: Panel;

  protected isRefreshError = false;

  constructor(
    private router: Router,
    private apiService: ApiService,
    private loggerService: ToastLoggerService,
  ) {}

  protected refresh() {
    if (!this._panelId) return;

    this.apiService.getApiV1Panel(this._panelId).subscribe({
      next: (data) => {
        this.isRefreshError = false;
        this.panel = {
          id: data.id || '',
          title: data.title || '',
          query: data.query?.query || '',
          dataSource: {
            id: data.query?.dataSource?.id || '',
            name: data.query?.dataSource?.name || '',
            driver: data.query?.dataSource?.type as DataSourceDriver,
            connectionUrl: data.query?.dataSource?.connectionString || '',
          },
          layout: decodeLayout(data.layout),
          visualizationType: VisualizationType.LINE,
          visualizationSettings: decodeVisualizationSettings(
            data.styleConfiguration,
          ),
        };
      },
      error: (e) => {
        this.loggerService.error(e);
        this.isRefreshError = true;
      },
    });
  }

  protected editPanel() {
    if (this._panelId) {
      this.router.navigateByUrl(urls.panelEditUrl(this._panelId));
    }
  }

  public refreshData() {
    const datepipe = new DatePipe('en-US');

    this.data = [
      ...this.data,
      {
        value: Math.random() * 10,
        timestamp: datepipe.transform(Date.now(), 'dd.MM HH:mm:ss') || '',
      },
    ];
  }

  public refreshTimeRange(timeRange: TimeRange) {
    if (this.panel && this.panel.dataSource) {
      this.apiService
        .getApiV1MetricsQueryRange(
          this.panel.dataSource.name,
          this.panel.query,
          'undefined' as any,
          null,
          timeRange.from,
          timeRange.to,
          timeRange.step,
        )
        .subscribe({
          next: (data) => {
            if (data.length !== 0) {
              const datepipe = new DatePipe('en-US');
              this.data =
                data[0].points?.map<DataPoint>((mp) => {
                  return {
                    value: mp.value || 0,
                    timestamp: datepipe.transform(mp.timestamp) || '',
                  };
                }) || [];
            } else {
              this.data = [];
            }
          },
          error: (e) => {
            this.loggerService.error(e);
          },
        });
    }
  }

  public saveLayout(): Observable<void> {
    if (this.panel) {
      const request = {
        title: this.panel.title,
        type: 1, // this.panel.visualizationType,
        rawQuery: this.panel.query,
        dataSourceId: this.panel.dataSource?.id,
        layout: encodeLayout(this.panel.layout),
        styleConfiguration: encodeVisualizationSettings(
          this.panel.visualizationSettings,
        ),
      } as any;
      return this.apiService.putApiV1PanelUpdate(this.panel.id, request);
    }
    return of(undefined);
  }

  public updateLayout(layout: Layout) {
    if (this.panel) {
      this.panel.layout = layout;
    }
  }
}
