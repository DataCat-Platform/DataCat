import { Component } from '@angular/core';
import { AccordionModule } from 'primeng/accordion';

@Component({
  standalone: true,
  selector: 'datacat-panel-visuals',
  templateUrl: './panel-visuals.component.html',
  imports: [AccordionModule],
})
export class PanelVisualsComponent {}
