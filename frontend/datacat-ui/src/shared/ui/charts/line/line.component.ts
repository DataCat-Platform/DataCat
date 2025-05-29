import { Component, ViewChild } from '@angular/core';
import { LineStyle, LineStyleScheme } from './line.style';
import { TimeSeries } from '../chart.types';
import { ChartModule, UIChart } from 'primeng/chart';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { ChartService } from '../chart.service';
import 'luxon';
import 'chartjs-adapter-luxon';

@Component({
  standalone: true,
  selector: 'datacat-line-chart',
  templateUrl: 'line.component.html',
  styleUrl: 'line.component.scss',
  imports: [ChartModule, ProgressSpinnerModule],
})
export class LineChartComponent {
  @ViewChild(UIChart) chart?: UIChart;

  protected isError: boolean = false;
  protected chartjsOptions: any;
  protected chartjsData: any;

  constructor(private chartService: ChartService) {
    this.chartService.style$.subscribe((style) => {
      this.updateStyle(style);
    });
    this.chartService.data$.subscribe((data) => {
      this.updateData(data);
    });
    this.chartService.error$.subscribe((error) => {
      this.isError = error;
    });
  }

  private updateStyle(style: any) {
    const parseResult = LineStyleScheme.safeParse(style);
    if (parseResult.success) {
      this.chartjsOptions = this.getChartjsOptionsFromLineStyle(
        parseResult.data,
      );
      this.chart?.chart?.update();
    }
  }

  private updateData(data: TimeSeries[]): void {
    this.chartjsData = {
      labels: [],
      datasets: data.map((ts) => {
        return {
          label: (() => {
            const { __name__, ...labels } = ts.labels;
            return JSON.stringify(labels);
          })(),
          data: ts.points.map((pt) => {
            return {
              x: pt.timestamp.toISOString(),
              y: pt.value,
            };
          }),
        };
      }),
    };
    this.chart?.chart?.update();
  }

  private getChartjsOptionsFromLineStyle(lineStyle: LineStyle): any {
    return {
      animation: false,
      maintainAspectRatio: false,
      plugins: {
        legend: {
          display: lineStyle.legend.enabled,
          position: lineStyle.legend.position,
        },
        title: {
          display: lineStyle.title.enabled,
          text: lineStyle.title.text,
        },
        tooltip: {
          enabled: lineStyle.tooltip.enabled,
        },
      },
      scales: {
        x: {
          type: 'time',
          min: lineStyle.axis.xAxisMin,
          max: lineStyle.axis.xAxisMax,
          title: {
            display: lineStyle.axis.xAxisTitle != undefined,
            text: lineStyle.axis.xAxisTitle,
          },
        },
        y: {
          min: lineStyle.axis.yAxisMin,
          max: lineStyle.axis.yAxisMax,
          title: {
            display: lineStyle.axis.yAxisTitle != undefined,
            text: lineStyle.axis.yAxisTitle,
          },
        },
      },
    };
  }

  protected hasData(): boolean {
    return this.chartjsData.datasets.length !== 0;
  }
}
