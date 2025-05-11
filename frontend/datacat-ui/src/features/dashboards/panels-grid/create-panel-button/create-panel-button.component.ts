import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { ApiService } from '../../../../shared/services/datacat-generated-client';
import { DialogModule } from 'primeng/dialog';
import { ToastLoggerService } from '../../../../shared/services/toast-logger.service';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { finalize } from 'rxjs';
import { InputTextModule } from 'primeng/inputtext';
import { TextareaModule } from 'primeng/textarea';
import { VisualizationType } from '../../../../entities';
import { DataSourceSelectComponent } from '../../../../shared/ui/data-source-select/data-source-select.component';
import { PanelModule } from 'primeng/panel';

@Component({
  standalone: true,
  selector: './datacat-create-panel-button',
  templateUrl: './create-panel-button.component.html',
  styleUrl: './create-panel-button.component.scss',
  imports: [
    ButtonModule,
    DialogModule,
    ReactiveFormsModule,
    InputTextModule,
    TextareaModule,
    DataSourceSelectComponent,
    PanelModule,
  ],
})
export class CreatePanelButtonComponent {
  @Output() onCreate = new EventEmitter<void>();

  @Input() public dashboardId?: string;

  protected isCreationDialogVisible = false;
  protected isCreationInitiated = false;

  protected creationForm = new FormGroup({
    title: new FormControl<string>('', Validators.required),
    rawQuery: new FormControl<string>('', Validators.required),
    dataSourceId: new FormControl<string | undefined>(
      undefined,
      Validators.required,
    ),
  });

  protected get titleControl() {
    return this.creationForm.get('title')!;
  }

  protected get queryControl() {
    return this.creationForm.get('rawQuery')!;
  }

  protected get dataSourceIdControl() {
    return this.creationForm.get('dataSourceId')!;
  }

  protected visualizationType = VisualizationType.LINE;
  protected visualizationTypes = Object.values(VisualizationType).filter(
    (t) => t !== VisualizationType.UNKNOWN,
  );

  constructor(
    private apiService: ApiService,
    private loggerService: ToastLoggerService,
  ) {}

  protected createPanel() {
    this.creationForm.markAllAsTouched();
    this.creationForm.updateValueAndValidity();

    if (this.creationForm.invalid || !this.dashboardId) return;

    this.isCreationInitiated = true;

    const request: any = this.creationForm.getRawValue();

    request.dashboardId = this.dashboardId;
    request.type = 1;

    this.apiService
      .postApiV1PanelAdd(request)
      .pipe(
        finalize(() => {
          this.isCreationInitiated = false;
        }),
      )
      .subscribe({
        next: () => {
          this.loggerService.success('Panel created');
          this.isCreationDialogVisible = false;
          this.creationForm.reset();
          this.onCreate.emit();
        },
        error: (e) => {
          this.loggerService.error(e);
        },
      });
  }
}
