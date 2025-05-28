import { Component } from '@angular/core';
import { ChartService } from '../chart.service';
import { VisualizationType } from '../../../../entities';
import { PanelModule } from 'primeng/panel';
import { AccordionModule } from 'primeng/accordion';
import { SelectModule } from 'primeng/select';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { LegendOptionsComponent } from './legend/legend-options.component';
import { TitleOptionsComponent } from './title/title-options.component';
import { TooltipOptionsComponent } from './tooltip/tooltip-options.component';
import { AxisOptionsComponent } from './axis/axis-options.component';

@Component({
  standalone: true,
  selector: 'datacat-chart-options',
  templateUrl: 'options.component.html',
  styleUrl: 'options.component.scss',
  imports: [
    ReactiveFormsModule,
    PanelModule,
    AccordionModule,
    SelectModule,
    LegendOptionsComponent,
    TitleOptionsComponent,
    TooltipOptionsComponent,
    AxisOptionsComponent,
  ],
})
export class ChartOptionsComponent {
  protected VisualizationType = VisualizationType;
  protected selectableTypes = Object.values(VisualizationType).filter(
    (t) => t !== VisualizationType.UNKNOWN,
  );
  protected typeControl = new FormControl<VisualizationType>(
    VisualizationType.LINE,
  );

  protected get type(): VisualizationType {
    return this.typeControl.value!;
  }

  constructor(private chartService: ChartService) {
    this.chartService.type$.subscribe((type) => {
      this.typeControl.setValue(type, { emitEvent: false });
    });
    this.typeControl.valueChanges.subscribe((type) => {
      if (type) {
        this.chartService.setType(type);
      }
    });
  }
}
