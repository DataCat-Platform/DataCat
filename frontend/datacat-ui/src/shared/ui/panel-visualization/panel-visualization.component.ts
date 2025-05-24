import { Component, Input, ViewChild } from '@angular/core';
import { VisualizationSettings, VisualizationType } from '../../../entities';
import { ChartModule } from 'primeng/chart';
import { BASIC_OPTIONS } from './consts';
import { TimeSeries } from '../../../entities/dashboards/data.types';
import { CommonModule, DatePipe } from '@angular/common';

@Component({
  standalone: true,
  selector: 'datacat-panel-vizualization',
  templateUrl: './panel-visualization.component.html',
  styleUrl: './panel-visualization.component.scss',
  imports: [ChartModule, CommonModule],
})
export class PanelVisualizationComponent {
  protected VisualizationType = VisualizationType;

  protected chartjsOptions: any = BASIC_OPTIONS;

  protected chartRef: any;

  protected chartjsData: any = {
    labels: [],
    datasets: [],
  };

  @ViewChild('chart') protected set chart(ref: any) {
    if (ref) {
      this.chartRef = ref;
      this.chartRef?.chart?.update();
    }
  }

  @Input() public visualizationType?: VisualizationType;

  @Input() public set data(data: TimeSeries[] | null) {
    if (data) {
      this.parseDataIntoChartjsData(data);
    }
  }

  @Input() public set visualizationSettings(
    settings: VisualizationSettings | undefined,
  ) {
    if (settings) {
      this.parseSettingsIntoChartjsOptions(settings);
    }
  }

  protected parseSettingsIntoChartjsOptions(settings: VisualizationSettings) {
    this.chartjsOptions.plugins.legend.display = settings.legend?.enabled;
    this.chartjsOptions.plugins.legend.position = settings.legend?.position;

    this.chartjsOptions.plugins.title.display = settings.title?.enabled;
    this.chartjsOptions.plugins.title.text = settings.title?.text;

    this.chartjsOptions.plugins.tooltip.enabled = settings.tooltip?.enabled;

    this.chartRef?.chart?.update();
  }

  protected parseDataIntoChartjsData(data: TimeSeries[]) {
    const datePipe = new DatePipe('en-US', undefined, {
      dateFormat: 'M/d/yy, h:mm a',
    });

    this.chartjsData = {
      labels:
        data[0]?.dataPoints.map((d) => datePipe.transform(d.timestamp)) || [],
      datasets: data.map((ts) => {
        return {
          label: ts.metric,
          data: ts.dataPoints.map((d) => d.value),
        };
      }),
    };

    this.chartRef?.chart?.update();
  }

  protected hasData(): boolean {
    if (this.chartjsData.datasets.length === 0) return false;

    for (const ds of this.chartjsData.datasets) {
      if (ds.data.length !== 0) return true;
    }

    return false;
  }
}
