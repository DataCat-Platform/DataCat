import { Component, Input } from '@angular/core';
import {
  FormArray,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
} from '@angular/forms';
import { from, timer } from 'rxjs';
import {
  FAKE_NOTIFICATION_CHANNEL,
  FAKE_NOTIFICATION_GROUP,
  getFakeNotificationChannel,
} from '../../../shared/mock/fakes';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { NotificationChannel } from '../../../entities';

@Component({
  standalone: true,
  selector: 'datacat-edit-notification-group-form',
  templateUrl: './edit-notification-group-form.component.html',
  styleUrl: './edit-notification-group-form.component.scss',
  imports: [ReactiveFormsModule, InputTextModule, ButtonModule],
})
export class EditNotificationGroupFormComponent {
  @Input() public set groupId(id: string) {
    this.loadEssentials(id);
  }

  protected isSavingInitiated = false;

  protected form = new FormGroup({
    name: new FormControl<string>(''),
    channels: new FormArray<FormGroup>([]),
  });

  protected get notificationChannels(): FormArray<FormGroup> {
    return this.form.get('channels') as FormArray<FormGroup>;
  }

  protected loadEssentials(groupId: string) {
    from([FAKE_NOTIFICATION_GROUP]).subscribe({
      next: (group) => {
        this.form.get('name')?.setValue(group.name);
      },
      error: () => {},
    });
    from([
      [getFakeNotificationChannel(), getFakeNotificationChannel()],
    ]).subscribe({
      next: () => {},
      error: () => {},
    });
  }

  protected saveNotificationGroup() {
    this.form.disable();
    this.isSavingInitiated = true;

    timer(1000).subscribe({
      next: () => {
        this.isSavingInitiated = false;
        this.form.enable();
      },
      error: () => {
        this.isSavingInitiated = false;
        this.form.enable();
      },
    });
  }

  protected addChannel() {
    this.notificationChannels.push(new FormGroup({
      address: new FormControl<string>(''),
    }));
  }
}
