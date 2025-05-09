import { Component, Input } from '@angular/core';
import { PanelModule } from 'primeng/panel';
import { EditPanelComponent } from '../../features/dashboards/edit-panel';

@Component({
  standalone: true,
  templateUrl: './manage-panel.component.html',
  styleUrl: './manage-panel.component.scss',
  imports: [PanelModule, EditPanelComponent],
})
export class ManagePanelComponent {
  @Input() public panelId?: string;
}
