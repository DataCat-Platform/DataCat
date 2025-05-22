import {
  afterNextRender,
  AfterViewInit,
  Component,
  Input,
  ViewChild,
} from '@angular/core';
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
  decodeVisualizationType,
  encodeVisualizationSettings,
  encodeVisualizationType,
  Panel,
  VisualizationSettings,
  VisualizationType,
} from '../../../entities';
import { ApiService } from '../../../shared/services/datacat-generated-client';
import { ToastLoggerService } from '../../../shared/services/toast-logger.service';
import { ButtonModule } from 'primeng/button';
import { finalize } from 'rxjs';
import { DataPoints } from '../../../entities/dashboards/data.types';

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
export class EditPanelComponent implements AfterViewInit {
  private _panelId?: string;

  @Input() public set panelId(id: string | undefined) {
    this._panelId = id;
    this.refresh();
  }

  @ViewChild(PanelVisualizationOptionsComponent)
  optionsComponent?: PanelVisualizationOptionsComponent;

  protected panel?: Panel;

  protected data: DataPoints = [];
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

  ngAfterViewInit() {
    if (this.panel) {
      this.optionsComponent?.setVisualizationSettings(
        this.panel.visualizationType!,
        this.panel.visualizationSettings!,
      );
    }
  }

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
          visualizationType: decodeVisualizationType(data.typeName),
          visualizationSettings: JSON.parse(
            data.styleConfiguration!,
          ) as VisualizationSettings,
        };

        this.optionsComponent?.setVisualizationSettings(
          this.panel.visualizationType!,
          this.panel.visualizationSettings!,
        );

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
      type: encodeVisualizationType(this.visualizationType),
      rawQuery: this.editForm.get('query')?.value || '',
      dataSourceId: this.editForm.get('dataSourceId')?.value || '',
      // layout: '',
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
