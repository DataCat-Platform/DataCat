import { Component, ViewChild } from '@angular/core';
import { PieStyle, PieStyleScheme } from './pie.style';
import { TimeSeries } from '../chart.types';
import { ChartModule, UIChart } from 'primeng/chart';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { ChartService } from '../chart.service';

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
      this.chart?.chart?.update();
    }
  }

  private updateData(data: TimeSeries[]): void {
    this.chartjsData = {
      labels: data.map((ts) => JSON.stringify(ts.labels)) || [],
      datasets: [
        {
          data:
            data.map((ts) => {
              return (
                ts.points.reduce(
                  (prev, curr) => {
                    return {
                      value: prev.value + curr.value,
                    };
                  },
                  { value: 0 },
                ).value / ts.points.length
              );
            }) || [],
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
}
