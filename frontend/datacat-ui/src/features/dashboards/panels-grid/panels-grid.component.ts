import { Component, Input } from '@angular/core';
import { Panel } from '../../../entities';

@Component({
  standalone: true,
  selector: 'datacat-panels-grid',
  templateUrl: './panels-grid.component.html',
  styleUrl: './panels-grid.component.scss',
})
export class PanelsGridComponent {
  @Input() panels: Panel[] = [];
}
