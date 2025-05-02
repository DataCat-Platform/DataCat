import { Component, OnInit } from '@angular/core';
import { PanelModule } from 'primeng/panel';
import { FormsModule } from '@angular/forms';
import { SelectButtonModule } from 'primeng/selectbutton';
import { SettingsService } from '../../features/settings/settings.service';
import { Divider } from 'primeng/divider';

@Component({
  selector: 'settings-page',
  standalone: true,
  imports: [PanelModule, FormsModule, SelectButtonModule, Divider],
  templateUrl: './settings.component.html',
  styleUrl: './settings.component.scss',
})
export class SettingsComponent {
  constructor(public settingsService: SettingsService) {}
}
