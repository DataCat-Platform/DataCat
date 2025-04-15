import { Component } from '@angular/core';
import { PanelModule } from 'primeng/panel';

@Component({
  selector: 'channels',
  standalone: true,
  imports: [PanelModule],
  templateUrl: './channels-explore-page.component.html',
})
export class ChannelsComponent {}
