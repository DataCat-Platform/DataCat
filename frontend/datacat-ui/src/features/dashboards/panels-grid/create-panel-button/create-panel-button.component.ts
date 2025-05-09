import { Component } from '@angular/core';
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
import { SelectModule } from 'primeng/select';
import {
  DataSource,
  VisualizationSettings,
  VisualizationType,
} from '../../../../entities';
import { ChipModule } from 'primeng/chip';
import { InputGroupModule } from 'primeng/inputgroup';
import { InputGroupAddonModule } from 'primeng/inputgroupaddon';
import { DataSourceSelectComponent } from '../../../../shared/ui/data-source-select/data-source-select.component';
import { InputMaskModule } from 'primeng/inputmask';
import { PanelVisualizationComponent } from '../../../../shared/ui/panel-visualization/panel-visualization.component';
import { PanelModule } from 'primeng/panel';
import { PanelVisualizationOptionsComponent } from '../../../../shared/ui/panel-visualization-options';

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
    SelectModule,
    ChipModule,
    InputGroupModule,
    InputGroupAddonModule,
    DataSourceSelectComponent,
    InputMaskModule,
    PanelVisualizationComponent,
    PanelModule,
    PanelVisualizationOptionsComponent,
  ],
})
export class CreatePanelButtonComponent {
  protected isCreationDialogVisible = false;
  protected isCreationInitiated = false;

  protected creationForm = new FormGroup({
    title: new FormControl<string | undefined>(undefined, {
      nonNullable: false,
      validators: Validators.required,
    }),
    type: new FormControl<VisualizationType>(VisualizationType.LINE, {
      nonNullable: false,
      validators: Validators.required,
    }),
    rawQuery: new FormControl<string>('', [Validators.required]),
    dataSourceId: new FormControl<string | undefined>(undefined, {
      nonNullable: false,
      validators: Validators.required,
    }),
  });

  protected dataSources: DataSource[] = [];

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

    if (this.creationForm.invalid) return;

    this.isCreationInitiated = true;

    const request: any = this.creationForm.getRawValue();
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
        },
        error: (e) => {
          this.loggerService.error(e);
        },
      });
  }

  protected setVisualizationType(type: VisualizationType) {
    this.visualizationType = type;
  }

  protected setVisualizationSettings(settings: VisualizationSettings) {}
}
