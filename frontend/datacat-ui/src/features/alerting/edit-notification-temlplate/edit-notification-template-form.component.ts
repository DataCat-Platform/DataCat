import { Component, Input } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { from, timer } from 'rxjs';
import {
  getFakeNotificationChannel,
  getFakeNotificationTemplate,
} from '../../../shared/mock/fakes';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { NotificationChannel, NotificationTemplate } from '../../../entities';
import { TextareaModule } from 'primeng/textarea';

@Component({
  standalone: true,
  selector: 'datacat-edit-notification-template-form',
  templateUrl: './edit-notification-template-form.component.html',
  styleUrl: './edit-notification-template-form.component.scss',
  imports: [ReactiveFormsModule, InputTextModule, ButtonModule, TextareaModule],
})
export class EditNotificationTemplateFormComponent {
  @Input() public set templateId(id: string) {
    this.loadEssentials(id);
  }

  protected isSavingInitiated = false;

  protected form = new FormGroup({
    name: new FormControl<string>(''),
    template: new FormControl<string>(''),
  });

  protected notificationTemplate?: NotificationTemplate;

  protected loadEssentials(templateId: string) {
    from([getFakeNotificationTemplate()]).subscribe({
      next: (template) => {
        this.form.get('name')?.setValue(template.name);
        this.form.get('template')?.setValue(template.template);
      },
      error: () => {},
    });
  }

  protected saveNotificationTemplate() {
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
}
