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

  @Input() public data: any;
  @Input() public visualizationType?: VisualizationType;
  @Input() public visualizationSettings?: VisualizationSettings;
}
