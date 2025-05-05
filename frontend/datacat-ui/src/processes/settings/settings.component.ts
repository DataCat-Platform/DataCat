import { Component } from '@angular/core';
import { PanelModule } from 'primeng/panel';
import { Divider } from 'primeng/divider';
import { SelectThemeComponent } from '../../features/appearence/select-theme/select-theme.component';

@Component({
  standalone: true,
  selector: 'datacat-settings',
  templateUrl: './settings.component.html',
  styleUrl: './settings.component.scss',
  imports: [PanelModule, Divider, SelectThemeComponent],
})
export class SettingsComponent {}
