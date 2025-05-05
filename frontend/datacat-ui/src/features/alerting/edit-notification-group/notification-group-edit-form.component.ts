import { Component, Input } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { from } from 'rxjs';
import { FAKE_NOTIFICATION_GROUP } from '../../../shared/mock/fakes';

@Component({
  standalone: true,
  selector: 'datacat-notification-group-edit-form',
  templateUrl: './notification-group-edit-form.component.html',
  imports: [ReactiveFormsModule],
})
export class NotificationGroupEditFormComponent {
  @Input() public set groupId(id: string) {
    this.loadEssentials(id);
  }

  protected form = new FormGroup({
    name: new FormControl<string>(''),
  });

  protected loadEssentials(groupId: string) {
    from([FAKE_NOTIFICATION_GROUP]).subscribe({});
  }
}
