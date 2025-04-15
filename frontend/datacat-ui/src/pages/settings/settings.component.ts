import { Component, OnInit } from '@angular/core';
import { PanelModule } from 'primeng/panel';
import { FormsModule } from '@angular/forms';
import { SelectButtonModule } from 'primeng/selectbutton';

@Component({
  selector: 'settings-page',
  standalone: true,
  imports: [PanelModule, FormsModule, SelectButtonModule],
  templateUrl: './settings.component.html',
})
export class SettingsPageComponent implements OnInit {
  theme!: string;
  themeOptions!: string[];

  ngOnInit() {
    this.theme = 'light';
    this.themeOptions = ['light', 'dark'];
  }

  onThemeChange() {
    const element = document.querySelector('html')!;
    if (this.theme == 'light') {
      element.classList.remove('use-dark-theme');
    } else {
      element.classList.add('use-dark-theme');
    }
  }
}
