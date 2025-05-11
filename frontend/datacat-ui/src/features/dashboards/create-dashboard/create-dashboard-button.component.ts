import { Component } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { ApiService } from '../../../shared/services/datacat-generated-client';
import { DialogModule } from 'primeng/dialog';
import { ToastLoggerService } from '../../../shared/services/toast-logger.service';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { finalize } from 'rxjs';
import { InputTextModule } from 'primeng/inputtext';
import { TextareaModule } from 'primeng/textarea';
import { SelectModule } from 'primeng/select';
import { ChipModule } from 'primeng/chip';
import { InputGroupModule } from 'primeng/inputgroup';
import { InputGroupAddonModule } from 'primeng/inputgroupaddon';
import { InputMaskModule } from 'primeng/inputmask';

@Component({
  standalone: true,
  selector: './datacat-create-dashboard-button',
  templateUrl: './create-dashboard-button.component.html',
  styleUrl: './create-dashboard-button.component.scss',
  imports: [
    ButtonModule,
    DialogModule,
    ReactiveFormsModule,
    InputTextModule,
    TextareaModule,
    SelectModule,
    ChipModule,
    InputGroupModule,
    InputGroupAddonModule,
    InputMaskModule,
  ],
})
export class CreateDashboardButtonComponent {
  protected isCreationDialogVisible = false;
  protected isCreationInitiated = false;

  protected addTagControl = new FormControl<string>('');

  protected creationForm = new FormGroup({
    name: new FormControl<string>('', Validators.required),
    description: new FormControl<string>('', Validators.required),
    tags: new FormControl<string[]>([]),
  });

  protected get nameControl() {
    return this.creationForm.get('name')!;
  }

  protected get descriptionControl() {
    return this.creationForm.get('description')!;
  }

  protected get tagsControl() {
    return this.creationForm.get('tags')!;
  }

  protected get creationFormTags(): string[] {
    return this.creationForm.get('tags')?.value || [];
  }

  protected addCreationFormTag() {
    const tag = this.addTagControl.value;
    const previousTags = this.creationForm.get('tags')?.value || [];
    if (tag && !previousTags.includes(tag)) {
      this.creationForm.get('tags')?.setValue([...previousTags, tag]);
    }
  }

  protected removeCreationFormTag(tag: string) {
    const previousTags = this.creationFormTags;
    this.creationForm
      .get('tags')
      ?.setValue(previousTags.filter((t) => t !== tag));
  }

  constructor(
    private apiService: ApiService,
    private loggerService: ToastLoggerService,
  ) {}

  protected createDashboard() {
    this.creationForm.markAllAsTouched();
    this.creationForm.updateValueAndValidity();

    if (this.creationForm.invalid) return;

    const rawForm = this.creationForm.getRawValue();
    const request: any = {
      name: rawForm.name,
      description: rawForm.description,
      tags: rawForm.tags,
    };

    this.isCreationInitiated = true;
    this.apiService
      .postApiV1DashboardAdd(request)
      .pipe(
        finalize(() => {
          this.isCreationInitiated = false;
        }),
      )
      .subscribe({
        next: () => {
          this.loggerService.success('Dashboard created');
          this.isCreationDialogVisible = false;
        },
        error: (e) => {
          this.loggerService.error(e);
        },
      });
  }
}
