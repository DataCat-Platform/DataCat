import { Component } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { timer } from 'rxjs';
import { ApiService } from '../../../shared/services/datacat-generated-client';
import { ToastLoggerService } from '../../../shared/services/toast-logger.service';
import { InputGroupAddonModule } from 'primeng/inputgroupaddon';
import { InputGroupModule } from 'primeng/inputgroup';
import { FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { InputTextModule } from 'primeng/inputtext';
import { durationValidator } from './mute-alert.validators';

@Component({
  standalone: true,
  selector: './datacat-mute-alert-button',
  templateUrl: './mute-alert-button.component.html',
  imports: [
    ButtonModule,
    InputTextModule,
    InputGroupAddonModule,
    InputGroupModule,
    ReactiveFormsModule,
  ],
})
export class MuteAlertButtonComponent {
  protected isMuteInitiated = false;

  protected muteTime = new FormControl<string>('', [
    Validators.required,
    durationValidator,
  ]);

  constructor(
    private apiService: ApiService,
    private loggerService: ToastLoggerService,
  ) {}

  protected muteAlert() {
    const duration = this.toDuration(this.muteTime.value);
    // TODO: call API
    this.isMuteInitiated = true;
    timer(1000).subscribe({
      next: () => {
        this.loggerService.success('Alert muted');
      },
      error: () => {
        this.loggerService.error('Unable to mute alert');
        this.isMuteInitiated = false;
      },
    });
  }

  protected toDuration(s: string | null): number {
    return 0;
  }
}
