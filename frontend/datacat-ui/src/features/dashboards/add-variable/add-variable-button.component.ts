import { Component, EventEmitter, Input, Output } from '@angular/core';
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
import { DataSource, NotificationGroup } from '../../../entities';
import { ChipModule } from 'primeng/chip';
import { InputGroupModule } from 'primeng/inputgroup';
import { InputGroupAddonModule } from 'primeng/inputgroupaddon';
import { InputMaskModule } from 'primeng/inputmask';

@Component({
  standalone: true,
  selector: './datacat-add-variable-button',
  templateUrl: './add-variable-button.component.html',
  styleUrl: './add-variable-button.component.scss',
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
export class AddVariableButtonComponent {
  @Input() public dashboardId?: string;
  @Output() public onAdd = new EventEmitter<void>();

  protected isCreationDialogVisible = false;
  protected isCreationInitiated = false;

  protected addTagControl = new FormControl<string>('');

  protected creationForm = new FormGroup({
    placeholder: new FormControl<string>('', Validators.required),
    value: new FormControl<string>('', Validators.required),
  });

  protected get placeholderControl() {
    return this.creationForm.get('placeholder')!;
  }

  protected get valueControl() {
    return this.creationForm.get('value')!;
  }

  constructor(
    private apiService: ApiService,
    private loggerService: ToastLoggerService,
  ) {}

  protected addVariable() {
    this.creationForm.markAllAsTouched();
    this.creationForm.updateValueAndValidity();

    if (this.creationForm.invalid || !this.dashboardId) return;

    this.isCreationInitiated = true;

    const request: any = this.creationForm.getRawValue();
    request.dashboardId = this.dashboardId;

    this.apiService
      .postApiV1VariableAdd(request)
      .pipe(
        finalize(() => {
          this.isCreationInitiated = false;
        }),
      )
      .subscribe({
        next: () => {
          this.loggerService.success('Variable added');
          this.isCreationDialogVisible = false;
          this.onAdd.emit();
        },
        error: (e) => {
          this.loggerService.error(e);
        },
      });
  }
}
