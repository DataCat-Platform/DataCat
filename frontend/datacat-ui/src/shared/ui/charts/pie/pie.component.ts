import { Component, ViewChild } from '@angular/core';
import { PieStyle, PieStyleScheme } from './pie.style';
import { TimeSeries } from '../chart.types';
import { ChartModule, UIChart } from 'primeng/chart';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { ChartService } from '../chart.service';
import { parse } from 'zod/v4';

@Component({
  standalone: true,
  selector: 'datacat-pie-chart',
  templateUrl: 'pie.component.html',
  styleUrl: 'pie.component.scss',
  imports: [ChartModule, ProgressSpinnerModule],
})
export class PieChartComponent {
  @ViewChild(UIChart) chart?: UIChart;

  protected isError: boolean = false;
  protected chartjsOptions: any;
  protected chartjsData: any;

  protected aggregationFunction: string = 'AVG';

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
    const parseResult = PieStyleScheme.safeParse(style);
    if (parseResult.success) {
      this.chartjsOptions = this.getChartjsOptionsFromPieStyle(
        parseResult.data,
      );
      this.aggregationFunction = parseResult.data.aggregate.function;
      this.updateData(this.chartService.data);
      this.chart?.chart?.update();
    }
  }

  private updateData(data: TimeSeries[]): void {
    this.chartjsData = {
      labels: data.map((ts) => JSON.stringify(ts.labels)) || [],
      datasets: [
        {
          data:
            data.map((ts) =>
              this.aggregateByType(this.aggregationFunction, ts),
            ) || [],
        },
      ],
    };
    this.chart?.chart?.update();
  }

  private getChartjsOptionsFromPieStyle(pieStyle: PieStyle): any {
    return {
      animation: false,
      maintainAspectRatio: false,
      plugins: {
        legend: {
          display: pieStyle.legend.enabled,
          position: pieStyle.legend.position,
        },
        title: {
          display: pieStyle.title.enabled,
          text: pieStyle.title.text,
        },
        tooltip: {
          enabled: pieStyle.tooltip.enabled,
        },
      },
    };
  }

  protected hasData(): boolean {
    return this.chartjsData.datasets.length !== 0;
  }

  protected aggregateByType(type: string, ts: TimeSeries): number {
    if (ts.points.length !== 0) {
      switch (type) {
        case 'AVG': {
          return (
            ts.points
              .map((pt) => pt.value)
              .reduce((sum, curr) => sum + curr, 0) / ts.points.length
          );
        }
        case 'MIN': {
          return ts.points
            .map((pt) => pt.value)
            .reduce(
              (min, curr) => (curr < min ? curr : min),
              ts.points[0].value,
            );
        }
        case 'MAX': {
          return ts.points
            .map((pt) => pt.value)
            .reduce(
              (max, curr) => (curr < max ? max : curr),
              ts.points[0].value,
            );
        }
      }
    }
    return 0;
  }
}
