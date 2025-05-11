import { Component, Input } from '@angular/core';
import { PanelModule } from 'primeng/panel';
import { EditPanelComponent } from '../../features/dashboards/edit-panel';
import { DeletePanelButtonComponent } from '../../features/dashboards/delete-panel';
import { Location } from '@angular/common';

@Component({
  standalone: true,
  templateUrl: './manage-panel.component.html',
  styleUrl: './manage-panel.component.scss',
  imports: [PanelModule, EditPanelComponent, DeletePanelButtonComponent],
})
export class ManagePanelComponent {
  @Input() public panelId?: string;

  constructor(private location: Location) {}

  protected gotoDashboard() {
    this.location.back();
  }
}
