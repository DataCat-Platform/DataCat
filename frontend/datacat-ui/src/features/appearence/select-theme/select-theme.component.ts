import { Component } from '@angular/core';
import { SelectButtonModule } from 'primeng/selectbutton';
import { ThemeSelectionService } from './select-theme.service';
import { FormsModule } from '@angular/forms';

@Component({
  standalone: true,
  selector: 'datacat-select-theme',
  templateUrl: './select-theme.component.html',
  imports: [SelectButtonModule, FormsModule],
})
export class SelectThemeComponent {
  constructor(protected themeSelectionService: ThemeSelectionService) {}
}
