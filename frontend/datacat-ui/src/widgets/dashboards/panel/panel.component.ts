import { Component } from '@angular/core';
import { PanelService } from '../../../features/dashboards/panel.service';

@Component({
  standalone: true,
  selector: 'datacat-dashboard-panel',
  templateUrl: './panel.component.html',
})
export class PanelComponent {
  constructor(protected panelService: PanelService) {}
}
