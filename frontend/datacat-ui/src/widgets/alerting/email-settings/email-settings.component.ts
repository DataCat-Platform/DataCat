import { Component, EventEmitter, Output } from '@angular/core';
import { EmailSettings } from '../../../entities';
import { InputTextModule } from 'primeng/inputtext';
import { FormsModule } from '@angular/forms';

@Component({
  standalone: true,
  selector: 'datacat-email-settings',
  templateUrl: './email-settings.component.html',
  imports: [InputTextModule, FormsModule],
})
export class EmailSettingsComponent {
  @Output('settings') protected settingsEventEmitter =
    new EventEmitter<EmailSettings>();
}
