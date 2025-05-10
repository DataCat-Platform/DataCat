import { Component, Input } from '@angular/core';
import { VisualizationSettings, VisualizationType } from '../../../entities';
import { ChartModule } from 'primeng/chart';

@Component({
  standalone: true,
  selector: 'datacat-panel-vizualization',
  templateUrl: './panel-visualization.component.html',
  styleUrl: './panel-visualization.component.scss',
  imports: [ChartModule],
})
export class PanelVisualizationComponent {
  protected VisualizationType = VisualizationType;

  @Input() public visualizationType?: VisualizationType;

  @Input() public set data(data: any) {}
  @Input() public set visualizationSettings(
    settings: VisualizationSettings | undefined,
  ) {
    if (settings) {
      this.chartjsOptions = this.parseSettingsIntoChartjsOptions(settings);
    }
  }

  protected parseSettingsIntoChartjsOptions(settings: VisualizationSettings) {
    this.chartjsOptions.plugins.legend.display = settings.legend?.enabled;
    this.chartjsOptions.plugins.legend.position = settings.legend?.position;

    switch (this.visualizationType) {
      case VisualizationType.LINE: {
        break;
      }
    }
  }

  protected chartjsData: any = {
    labels: ['1', '2', '3'],
    datasets: [
      {
        order: 0,
        label: 'Label 1',
        data: [1, 8, 3],
        // borderColor: 'red',
        // backgroundColor: 'blue',
      },
    ],
  };

  protected chartjsOptions: any = {
    animation: false,
    maintainAspectRatio: false,
    scales: {
      x: {
        min: 0,
        max: 10,
        ticks: {},
      },
      y: {
        min: 0,
        max: 10,
        ticks: {},
      },
    },
    plugins: {
      legend: {
        display: true,
        position: 'top',
      },
      title: {
        display: false,
        text: '',
      },
      tooltip: {
        enabled: true,
      },
    },
    parsing: {
      key: 'x',
      xAxisKey: 'x',
      yAxisKey: 'y',
    },
    layout: {
      padding: null,
    },
  };
}
