import { Component } from '@angular/core';
import { PanelModule } from 'primeng/panel';

@Component({
  selector: 'datacat-installed-plugins',
  standalone: true,
  imports: [PanelModule],
  templateUrl: './installed-plugins.component.html',
})
export class InstalledPluginsComponent {}
