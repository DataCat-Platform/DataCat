import { Component, OnInit } from '@angular/core';
import { PanelModule } from 'primeng/panel';
import { FormsModule } from '@angular/forms';
import { SelectButtonModule } from 'primeng/selectbutton';
import { SettingsService } from '../../features/settings/settings.service';

@Component({
  selector: 'settings-page',
  standalone: true,
  imports: [PanelModule, FormsModule, SelectButtonModule],
  templateUrl: './settings.component.html',
})
export class SettingsPageComponent {
  constructor(public settingsService: SettingsService) {}
}
