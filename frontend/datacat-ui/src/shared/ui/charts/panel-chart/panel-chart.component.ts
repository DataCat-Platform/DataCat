import { Component, Host, Input } from '@angular/core';
import { VisualizationType } from '../../../../entities';
import { LineChartComponent } from '../line/line.component';
import { UnsupportedChartComponent } from '../unsupported/unsupported.component';
import { ChartService } from '../chart.service';
import { ProgressSpinnerModule } from 'primeng/progressspinner';

@Component({
  standalone: true,
  selector: 'datacat-panel-chart',
  templateUrl: 'panel-chart.component.html',
  imports: [
    ProgressSpinnerModule,
    LineChartComponent,
    UnsupportedChartComponent,
  ],
})
export class PanelChartComponent {
  @Input() public loading: boolean = false;

  protected type?: VisualizationType;
  protected VisualizationType = VisualizationType;

  constructor(private chartService: ChartService) {
    this.chartService.type$.subscribe((type) => {
      this.type = type;
    });
  }
}
