import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { TextareaModule } from 'primeng/textarea';
import * as urls from '../../../shared/common/urls';
import { ApiService } from '../../../shared/services/api.service';
import { timer } from 'rxjs';

@Component({
  standalone: true,
  selector: './datacat-create-alert-button',
  templateUrl: './create-alert-button.component.html',
  styleUrl: './create-alert-button.component.scss',
  imports: [ButtonModule, TextareaModule],
})
export class CreateAlertButtonComponent {
  protected isCreationInitiated = false;

  constructor(
    private router: Router,
    private apiService: ApiService,
  ) {}

  protected createAlert() {
    // TODO: call API
    this.isCreationInitiated = true;
    timer(1000).subscribe({
      next: () => {
        const alertId = '0';
        this.router.navigateByUrl(urls.alertEditUrl(alertId));
      },
      error: () => {
        // TODO
        this.isCreationInitiated = false;
      }
    })

  }
}
