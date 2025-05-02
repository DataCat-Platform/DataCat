import { Component } from '@angular/core';
import { PanelModule } from 'primeng/panel';

@Component({
  standalone: true,
  selector: 'datacat-home',
  imports: [PanelModule],
  templateUrl: './home.component.html',
})
export class HomeComponent {}
