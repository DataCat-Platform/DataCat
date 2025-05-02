import { Component, Input } from '@angular/core';
import { Panel } from 'primeng/panel';
import { CommonModule } from '@angular/common';
import { TabsModule } from 'primeng/tabs';
import { TextareaModule } from 'primeng/textarea';
import { Alert } from '../../../entities/alerts.entities';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { FormsModule } from '@angular/forms';
import { TagModule } from 'primeng/tag';
import { InputGroup } from 'primeng/inputgroup';
import { InputGroupAddon } from 'primeng/inputgroupaddon';
import { InputText } from 'primeng/inputtext';
import { SplitButtonModule } from 'primeng/splitbutton';

@Component({
  standalone: true,
  selector: 'datacat-alert-edit',
  templateUrl: './alert-edit.component.html',
  styleUrl: './alert-edit.component.scss',
  imports: [
    CommonModule,
    Panel,
    TabsModule,
    TextareaModule,
    ProgressSpinnerModule,
    FormsModule,
    TagModule,
    InputGroup,
    InputGroupAddon,
    SplitButtonModule,
  ],
})
export class AlertEditComponent {
  @Input() alertId: string = '';

  protected alert?: Alert;

  constructor() { //protected alertEditService: AlertEditService
    // alertEditService.alert$.subscribe((alert) => (this.alert = alert));
  }
}
