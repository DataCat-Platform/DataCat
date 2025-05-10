import { Component, Input } from '@angular/core';
import { PanelModule } from 'primeng/panel';
import { PanelVisualizationComponent } from '../../../../shared/ui/panel-visualization/panel-visualization.component';
import {
  DataSourceDriver,
  decodeLayout,
  decodeVisualizationSettings,
  Panel,
  VisualizationType,
} from '../../../../entities';
import { ApiService } from '../../../../shared/services/datacat-generated-client';
import { ToastLoggerService } from '../../../../shared/services/toast-logger.service';
import { ButtonModule } from 'primeng/button';
import { Router } from '@angular/router';
import * as urls from '../../../../shared/common/urls';
import { DeletePanelButtonComponent } from '../../delete-panel';

@Component({
  standalone: true,
  selector: 'datacat-panel-in-grid',
  templateUrl: './panel-in-grid.component.html',
  styleUrl: './panel-in-grid.component.scss',
  imports: [
    PanelModule,
    PanelVisualizationComponent,
    ButtonModule,
  ],
})
export class PanelInGridComponent {
  private _panelId?: string;

  @Input() set panelId(id: string) {
    this._panelId = id;
    this.refresh();
  }

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
          visualizationSetttings: decodeVisualizationSettings(
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
}
