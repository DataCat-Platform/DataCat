import { Component, Input, ViewChild } from '@angular/core';
import { VisualizationSettings, VisualizationType } from '../../../entities';
import { ChartModule } from 'primeng/chart';
import { BASIC_OPTIONS } from './consts';
import { DataPoints } from '../../../entities/dashboards/data.types';
import { ThemeProvider } from 'primeng/config';

@Component({
  standalone: true,
  selector: 'datacat-panel-vizualization',
  templateUrl: './panel-visualization.component.html',
  styleUrl: './panel-visualization.component.scss',
  imports: [ChartModule],
})
export class PanelVisualizationComponent {
  protected VisualizationType = VisualizationType;

  protected chartjsOptions: any = BASIC_OPTIONS;

  protected chartRef: any;

  @ViewChild('chart') protected set chart(ref: any) {
    this.chartRef = ref;
    this.chartRef?.chart?.update();
  }

  @Input() public visualizationType?: VisualizationType;

  @Input() public set data(data: DataPoints) {
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
    const chart: any = this.chartRef?.chart;

    this.chartjsOptions.plugins.legend.display = settings.legend?.enabled;
    this.chartjsOptions.plugins.legend.position = settings.legend?.position;

    this.chartjsOptions.plugins.title.display = settings.title?.enabled;
    this.chartjsOptions.plugins.title.text = settings.title?.text;

    chart?.update();
  }

  protected parseDataIntoChartjsData(data: DataPoints) {
    this.chartjsData = {
      labels: data.map((d) => d.timestamp),
      datasets: [
        {
          label: 'Label',
          data: data.map((d) => d.value),
        },
      ],
    };

    this.chart?.update();
  }

  protected chartjsData: any = {
    labels: ['1', '2', '3', '4', '5', '6'],
    datasets: [
      {
        order: 0,
        label: 'Label 1',
        data: [1, 8, 3, 2, 5, 10],
        // borderColor: 'red',
        // backgroundColor: 'blue',
      },
    ],
  };
}
