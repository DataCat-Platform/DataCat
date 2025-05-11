import { Component, Input } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { finalize, timer } from 'rxjs';
import { ApiService } from '../../../shared/services/datacat-generated-client';
import { ToastLoggerService } from '../../../shared/services/toast-logger.service';
import { FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { InputMaskModule } from 'primeng/inputmask';

@Component({
  standalone: true,
  selector: './datacat-mute-alert-button',
  templateUrl: './mute-alert-button.component.html',
  styleUrl: './mute-alert-button.component.scss',
  imports: [ButtonModule, ReactiveFormsModule, InputMaskModule],
})
export class MuteAlertButtonComponent {
  @Input({ required: true }) alertId?: string;

  protected isMuteInitiated = false;

  protected nextExecutionTimeControl = new FormControl<string>('', [
    Validators.required,
  ]);

  constructor(
    private apiService: ApiService,
    private loggerService: ToastLoggerService,
  ) {}

  protected get nextExecutionTime(): string | null {
    return this.nextExecutionTimeControl.value;
  }

  protected muteAlert() {
    this.nextExecutionTimeControl.markAsTouched();
    this.nextExecutionTimeControl.updateValueAndValidity();

    if (
      !this.alertId ||
      !this.nextExecutionTime ||
      !this.nextExecutionTimeControl.valid
    ) {
      return;
    }

    this.isMuteInitiated = true;
    this.apiService
      .postApiV1AlertMute(this.alertId, this.nextExecutionTime)
      .pipe(
        finalize(() => {
          this.isMuteInitiated = false;
        }),
      )
      .subscribe({
        next: () => {
          this.loggerService.success('Alert muted');
        },
        error: (e) => {
          this.loggerService.error(e);
        },
      });
  }
}
