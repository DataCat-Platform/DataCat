import { Component, Host, Input, OnInit, ViewChild } from '@angular/core';
import { LineStyle, LineStyleScheme } from './line.style';
import { TimeSeries } from '../chart.types';
import { ChartModule, UIChart } from 'primeng/chart';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { ChartService } from '../chart.service';

@Component({
  standalone: true,
  selector: 'datacat-line-chart',
  templateUrl: 'line.component.html',
  styleUrl: 'line.component.scss',
  imports: [ChartModule, ProgressSpinnerModule],
})
export class LineChartComponent implements OnInit {
  @ViewChild(UIChart) chart?: UIChart;

  protected chartjsOptions: any;
  protected chartjsData: any;

  constructor(private chartService: ChartService) {
    this.chartService.style$.subscribe((style) => {
      this.updateStyle(style);
    });
    this.chartService.data$.subscribe((data) => {
      this.updateData(data);
    });
  }

  ngOnInit() {
    this.chartjsData = {
      labels: [],
      datasets: [],
    };
    this.chartjsOptions = {};
  }

  private updateStyle(style: any) {
    const parseResult = LineStyleScheme.safeParse(style);
    if (parseResult.success) {
      this.chartjsOptions = this.getChartjsOptionsFromLineStyle(
        parseResult.data,
      );
    }
  }

  private updateData(data: TimeSeries[]) {
    this.chartjsData = {
      labels: [],
      datasets: [],
    };
  }

  private getChartjsOptionsFromLineStyle(lineStyle: LineStyle) {}
}
