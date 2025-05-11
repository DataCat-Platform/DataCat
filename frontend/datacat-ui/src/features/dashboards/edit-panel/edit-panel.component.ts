import { afterNextRender, Component, Input } from '@angular/core';
import { PanelVisualizationComponent } from '../../../shared/ui/panel-visualization';
import { PanelVisualizationOptionsComponent } from '../../../shared/ui/panel-visualization-options';
import { PanelModule } from 'primeng/panel';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { InputTextModule } from 'primeng/inputtext';
import { TextareaModule } from 'primeng/textarea';
import { DataSourceSelectComponent } from '../../../shared/ui/data-source-select/data-source-select.component';
import {
  DataSourceDriver,
  decodeLayout,
  encodeVisualizationSettings,
  Panel,
  VisualizationSettings,
  VisualizationType,
} from '../../../entities';
import { ApiService } from '../../../shared/services/datacat-generated-client';
import { ToastLoggerService } from '../../../shared/services/toast-logger.service';
import { ButtonModule } from 'primeng/button';
import { finalize } from 'rxjs';

@Component({
  standalone: true,
  selector: 'datacat-edit-panel',
  templateUrl: './edit-panel.component.html',
  styleUrl: './edit-panel.component.scss',
  imports: [
    PanelVisualizationComponent,
    PanelVisualizationOptionsComponent,
    PanelModule,
    ReactiveFormsModule,
    InputTextModule,
    TextareaModule,
    DataSourceSelectComponent,
    ButtonModule,
  ],
})
export class EditPanelComponent {
  private _panelId?: string;

  @Input() public set panelId(id: string | undefined) {
    this._panelId = id;
    this.refresh();
  }

  protected panel?: Panel;

  protected data: any = {
    labels: ['1', '2', '3', '4', '5', '6', '7'],
    datasets: [
      {
        label: 'First Dataset',
        data: [65, 59, 80, 81, 56, 55, 40],
      },
    ],
  };
  protected visualizationType?: VisualizationType;
  protected visualizationSettings?: VisualizationSettings;

  protected editForm = new FormGroup({
    title: new FormControl<string>('', Validators.required),
    dataSourceId: new FormControl<string | undefined>(
      undefined,
      Validators.required,
    ),
    query: new FormControl<string>('', Validators.required),
  });

  constructor(
    private apiService: ApiService,
    private loggerService: ToastLoggerService,
  ) {}

  protected refresh() {
    if (!this._panelId) return;

    this.apiService.getApiV1Panel(this._panelId).subscribe({
      next: (data) => {
        this.panel = {
          id: data.id || '',
          title: data.title || '',
          query: data.query?.query || '',
          dataSource: {
            id: data.query?.dataSource?.id || '',
            name: data.query?.dataSource?.name || '',
            driver: data.query?.dataSource?.type as DataSourceDriver,
            connectionUrl: data.query?.dataSource?.connectionString || '',
          },
          layout: decodeLayout(data.layout),
          visualizationType: data.typeName as VisualizationType,
          visualizationSettings:
            data.styleConfiguration as VisualizationSettings,
        };

        this.editForm.setValue({
          title: this.panel.title,
          dataSourceId: this.panel.dataSource?.id,
          query: this.panel.query,
        });
      },
      error: (e) => {
        this.loggerService.error(e);
      },
    });
  }

  protected saveChanges() {
    if (!this._panelId) return;

    const request: any = {
      title: this.editForm.get('title')?.value || '',
      type: this.visualizationType,
      rawQuery: this.editForm.get('query')?.value || '',
      dataSourceId: this.editForm.get('dataSourceId')?.value || '',
      layout: '',
      styleConfiguration: encodeVisualizationSettings(
        this.visualizationSettings,
      ),
    };

    this.editForm.disable();
    this.apiService
      .putApiV1PanelUpdate(this._panelId, request)
      .pipe(finalize(() => this.editForm.enable()))
      .subscribe({
        next: () => {
          this.loggerService.success('Saved');
        },
        error: (e) => {
          this.loggerService.error(e);
        },
      });
  }
}
