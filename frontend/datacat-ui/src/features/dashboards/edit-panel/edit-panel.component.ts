import { AfterViewInit, Component, Input, ViewChild } from '@angular/core';
import { PanelVisualizationOptionsComponent } from '../../../shared/ui/panel-visualization-options';
import { PanelModule } from 'primeng/panel';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { InputTextModule } from 'primeng/inputtext';
import { TextareaModule } from 'primeng/textarea';
import { DataSourceSelectComponent } from '../../../shared/ui/data-source-select/data-source-select.component';
import {
  DataSourceDriver,
  decodeLayout,
  decodeVisualizationType,
  encodeVisualizationSettings,
  encodeVisualizationType,
  Panel,
  serializeLayout,
  VisualizationSettings,
  VisualizationType,
} from '../../../entities';
import { ApiService } from '../../../shared/services/datacat-generated-client';
import { ToastLoggerService } from '../../../shared/services/toast-logger.service';
import { ButtonModule } from 'primeng/button';
import { finalize, timer } from 'rxjs';
import { TimeSeries } from '../../../entities/dashboards/data.types';
import { PanelDataService } from '../panels-grid/panel-data.service';
import { TimeRangeSelectComponent } from '../../../shared/ui/time-range-select/time-range-select.component';
import { TimeRange } from '../../../entities/dashboards/etc.types';
import { PanelChartComponent } from '../../../shared/ui/charts/panel-chart/panel-chart.component';
import { ChartService } from '../../../shared/ui/charts/chart.service';
import { ChartOptionsComponent } from '../../../shared/ui/charts/options/options.component';

@Component({
  standalone: true,
  selector: 'datacat-edit-panel',
  templateUrl: './edit-panel.component.html',
  styleUrl: './edit-panel.component.scss',
  imports: [
    PanelModule,
    ReactiveFormsModule,
    InputTextModule,
    TextareaModule,
    DataSourceSelectComponent,
    ButtonModule,
    TimeRangeSelectComponent,
    PanelChartComponent,
    ChartOptionsComponent,
  ],
  providers: [ChartService],
})
export class EditPanelComponent {
  private _panelId?: string;

  @Input() public set panelId(id: string | undefined) {
    this._panelId = id;
    this.refresh();
  }

  protected panel?: Panel;

  protected data: TimeSeries[] | null = null;
  protected visualizationType?: VisualizationType;
  protected visualizationSettings?: VisualizationSettings;

  protected timeRangeControl = new FormControl<TimeRange>({
    step: '00:30:00',
    from: (() => {
      const date = new Date();
      date.setTime(date.getTime() - 4 * 60 * 60 * 1000);
      date.setMilliseconds(0);
      return date;
    })(),
    to: (() => {
      const date = new Date();
      date.setMilliseconds(0);
      return date;
    })(),
  });

  protected editForm = new FormGroup({
    title: new FormControl<string>('', Validators.required),
    dataSourceId: new FormControl<string | undefined>(
      undefined,
      Validators.required,
    ),
    query: new FormControl<string>('', Validators.required),
  });

  constructor(
    private api: ApiService,
    private logger: ToastLoggerService,
    private chartService: ChartService,
  ) {
    this.timeRangeControl.valueChanges.subscribe((tr) => {
      this.refreshPreview();
    });
    this.editForm.get('dataSourceId')?.valueChanges.subscribe((id) => {
      if (id && this.panel) {
        this.panel.dataSource!.id = id;
        this.chartService.setDataSourceName(this.panel.dataSource!.name);
        this.refreshPreview();
      }
    });
    this.editForm.get('query')?.valueChanges.subscribe((q) => {
      if (q && this.panel) {
        this.panel.query = q;
        this.chartService.setQuery(this.panel.query);
        this.refreshPreview();
      }
    });
  }

  protected refresh() {
    if (!this._panelId) return;

    this.api.getApiV1Panel(this._panelId).subscribe({
      next: (data) => {
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
          visualizationType: decodeVisualizationType(data.typeName),
          visualizationSettings: JSON.parse(
            data.styleConfiguration!,
          ) as VisualizationSettings,
        };
        this.chartService.setQuery(this.panel.query);
        this.chartService.setDataSourceName(this.panel.dataSource!.name);
        this.chartService.updateStyle(this.panel.visualizationSettings);
        this.refreshPreview();

        this.editForm.setValue({
          title: this.panel.title,
          dataSourceId: this.panel.dataSource?.id,
          query: this.panel.query,
        });
      },
      error: () => {
        this.logger.error('Cannot load panel data');
      },
    });
  }

  protected refreshPreview() {
    const timeRange = this.timeRangeControl.getRawValue()!;
    this.chartService.loadTimeRange(
      timeRange.from,
      timeRange.to,
      timeRange.step,
    );
  }

  protected saveChanges() {
    if (!this.panel) return;

    const request: any = {
      title: this.editForm.get('title')?.value || '',
      type: encodeVisualizationType(this.chartService.type),
      rawQuery: this.editForm.get('query')?.value || '',
      dataSourceId: this.editForm.get('dataSourceId')?.value || '',
      layout: serializeLayout(this.panel.layout),
      styleConfiguration: encodeVisualizationSettings(this.chartService.style),
    };

    this.editForm.disable();
    this.api
      .putApiV1PanelUpdate(this.panel.id, request)
      .pipe(finalize(() => this.editForm.enable()))
      .subscribe({
        next: () => {
          this.logger.success('Saved');
        },
        error: () => {
          this.logger.error('Cannot save');
        },
      });
  }
}
