import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import * as urls from '../../../../shared/common/urls';

/**
 * Button that navigates to alert editing activity.
 */
@Component({
  standalone: true,
  selector: 'datacat-goto-edit-alert-button',
  templateUrl: './goto-edit-alert-button.component.html',
  imports: [ButtonModule],
})
export class GoToEditAlertButtonComponent {
  @Input() alertId?: string;

  constructor(private router: Router) {}

  protected editAlert() {
    if (this.alertId) {
      this.router.navigateByUrl(urls.alertEditUrl(this.alertId));
    }
  }
}
