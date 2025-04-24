import { Component, Input } from '@angular/core';
import { PanelModule } from 'primeng/panel';
import { Panel } from '../../entities/dashboard.entities';

@Component({
  standalone: true,
  selector: 'datacat-dashboard-panel',
  templateUrl: './panel.component.html',
  imports: [PanelModule],
})
export class PanelComponent {
  @Input() panel?: Panel;
}
