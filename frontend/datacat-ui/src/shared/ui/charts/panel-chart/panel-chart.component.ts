import { Component, Input } from '@angular/core';
import { VisualizationType } from '../../../../entities';
import { LineChartComponent } from '../line/line.component';
import { UnsupportedChartComponent } from '../unsupported/unsupported.component';
import { ChartService } from '../chart.service';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { BarChartComponent } from '../bar/bar.component';
import { PieChartComponent } from '../pie/pie.component';

@Component({
  standalone: true,
  selector: 'datacat-panel-chart',
  templateUrl: 'panel-chart.component.html',
  styleUrl: 'panel-chart.component.scss',
  imports: [
    ProgressSpinnerModule,
    LineChartComponent,
    BarChartComponent,
    PieChartComponent,
    UnsupportedChartComponent,
  ],
})
export class PanelChartComponent {
  protected loading: boolean = false;

  protected type?: VisualizationType;
  protected VisualizationType = VisualizationType;

  constructor(private chartService: ChartService) {
    this.chartService.type$.subscribe((type) => {
      this.type = type;
    });
    this.chartService.isLoading$.subscribe((isLoading) => {
      this.loading = isLoading;
    });
  }
}
