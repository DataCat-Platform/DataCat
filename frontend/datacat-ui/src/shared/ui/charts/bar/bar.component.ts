import { Component, ViewChild } from '@angular/core';
import { BarStyle, BarStyleScheme } from './bar.style';
import { TimeSeries } from '../chart.types';
import { ChartModule, UIChart } from 'primeng/chart';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { ChartService } from '../chart.service';

@Component({
  standalone: true,
  selector: 'datacat-bar-chart',
  templateUrl: 'bar.component.html',
  styleUrl: 'bar.component.scss',
  imports: [ChartModule, ProgressSpinnerModule],
})
export class BarChartComponent {
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
    const parseResult = BarStyleScheme.safeParse(style);
    if (parseResult.success) {
      this.chartjsOptions = this.getChartjsOptionsFromBarStyle(
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
          label: JSON.stringify(ts.labels),
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

  private getChartjsOptionsFromBarStyle(barStyle: BarStyle): any {
    return {
      animation: false,
      maintainAspectRatio: false,
      plugins: {
        legend: {
          display: barStyle.legend.enabled,
          position: barStyle.legend.position,
        },
        title: {
          display: barStyle.title.enabled,
          text: barStyle.title.text,
        },
        tooltip: {
          enabled: barStyle.tooltip.enabled,
        },
      },
      scales: {
        x: {
          type: 'time',
          min: barStyle.axis.xAxisMin,
          max: barStyle.axis.xAxisMax,
          title: {
            display: barStyle.axis.xAxisTitle != undefined,
            text: barStyle.axis.xAxisTitle,
          },
        },
        y: {
          min: barStyle.axis.yAxisMin,
          max: barStyle.axis.yAxisMax,
          title: {
            display: barStyle.axis.yAxisTitle != undefined,
            text: barStyle.axis.yAxisTitle,
          },
        },
      },
    };
  }

  protected hasData(): boolean {
    return this.chartjsData.datasets.length !== 0;
  }
}
