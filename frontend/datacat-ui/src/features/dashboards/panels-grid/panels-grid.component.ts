import { Component, Input, QueryList, ViewChildren } from '@angular/core';
import {
  DashboardVariable,
  decodeLayout,
  encodeLayout,
  encodeVisualizationSettings,
  Panel,
  PanelType,
  VisualizationType,
} from '../../../entities';
import { ApiService } from '../../../shared/services/datacat-generated-client';
import { ToastLoggerService } from '../../../shared/services/toast-logger.service';
import { ButtonModule } from 'primeng/button';
import { SelectModule } from 'primeng/select';
import { InputGroupModule } from 'primeng/inputgroup';
import { InputGroupAddonModule } from 'primeng/inputgroupaddon';
import { ToggleButtonModule } from 'primeng/togglebutton';
import { finalize, forkJoin, interval, Subscription, timer } from 'rxjs';
import {
  DisplayGrid,
  GridsterConfig,
  GridsterItem,
  GridsterItemComponentInterface,
  GridsterModule,
  GridType,
} from 'angular-gridster2';
import { CardModule } from 'primeng/card';
import { PanelModule } from 'primeng/panel';
import { PanelInGridComponent } from './panel-in-grid';
import { CreatePanelButtonComponent } from './create-panel-button';
import { RefreshRateOption } from '.';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { AddVariableButtonComponent } from '../add-variable';
import { DeleteVariableButtonComponent } from '../delete-variable';
import { DatePickerModule } from 'primeng/datepicker';
import {
  TimeRange,
  TimeRangeSelectComponent,
} from '../../../shared/ui/time-range-select';

@Component({
  standalone: true,
  selector: 'datacat-panels-grid',
  templateUrl: './panels-grid.component.html',
  styleUrl: './panels-grid.component.scss',
  imports: [
    ButtonModule,
    SelectModule,
    InputGroupModule,
    InputGroupAddonModule,
    GridsterModule,
    CardModule,
    PanelModule,
    ToggleButtonModule,
    PanelInGridComponent,
    CreatePanelButtonComponent,
    ReactiveFormsModule,
    AddVariableButtonComponent,
    DeleteVariableButtonComponent,
    DatePickerModule,
    TimeRangeSelectComponent,
  ],
})
export class PanelsGridComponent {
  @ViewChildren(PanelInGridComponent)
  public panelsComponents!: QueryList<PanelInGridComponent>;

  constructor(
    private apiService: ApiService,
    private loggerService: ToastLoggerService,
  ) {
    this.freezeGrid();
    this.refreshRateControl.valueChanges.subscribe((seconds) =>
      this.setRefreshRate(seconds),
    );
    this.timeRangeControl.valueChanges.subscribe((timeRange) => {
      this.updateTimeRange(timeRange);
    });
  }

  protected _dashboardId?: string;

  @Input() public set dashboardId(id: string) {
    this._dashboardId = id;
    this.refreshDashboard();
    this.refreshDashboardVariables();
    this.refreshPanelTypes();
  }

  protected isSaving = false;
  protected isBusy = false;

  protected refreshRateOptions: RefreshRateOption[] = [
    { title: 'off', seconds: null },
    { title: '10s', seconds: 10 },
    { title: '30s', seconds: 30 },
    { title: '1m', seconds: 60 },
    { title: '10m', seconds: 600 },
    { title: '1h', seconds: 3600 },
  ];
  protected refreshRateControl = new FormControl<number | null>(null);
  protected refreshRateSubscription?: Subscription;
  protected timeRangeControl = new FormControl<TimeRange | null>(null);

  protected variables: DashboardVariable[] = [];

  protected panels: Panel[] = [];

  protected panelTypes: PanelType[] = [];

  protected gridsterItems: GridsterItem[] = [];
  protected gridsterOptions: GridsterConfig = {
    gridType: GridType.Fixed,
    displayGrid: DisplayGrid.None,
    pushItems: true,
    swap: false,
    fixedColWidth: 50,
    fixedRowHeight: 50,
    draggable: {
      enabled: true,
    },
    resizable: {
      enabled: true,
    },
    enableBoundaryControl: true,
    itemChangeCallback: this.handleGridsterItemChange.bind(this),
  };

  protected refreshPanelTypes() {
    this.apiService.getApiV1PanelTypes().subscribe({
      next: (data) => {
        this.panelTypes = data.map<PanelType>((d) => {
          return {
            id: d.id || 0,
            type: d.name as VisualizationType,
          };
        });
      },
      error: (e) => {
        this.loggerService.error(e);
      },
    });
  }

  protected refreshDashboard() {
    if (!this._dashboardId) return;

    this.isBusy = true;
    this.refreshRateControl.disable();
    this.apiService
      .getApiV1DashboardFull(this._dashboardId)
      .pipe(
        finalize(() => {
          this.isBusy = false;
          this.refreshRateControl.enable();
        }),
      )
      .subscribe({
        next: (data) => {
          this.panels =
            data.panels?.map<Panel>((item) => {
              return {
                id: item.id || '',
                title: item.title || '',
                query: item.query || '',
                visualizationType: item.panelType as VisualizationType,
                layout: decodeLayout(item.layout),
              };
            }) || [];
          this.refreshGridsterItems();
        },
        error: (e) => {
          this.loggerService.error(e);
        },
      });
  }

  protected refreshDashboardVariables() {
    if (!this._dashboardId) return;

    this.apiService.getApiV1VariablesDashboard(this._dashboardId).subscribe({
      next: (data) => {
        this.variables = data.map<DashboardVariable>((item) => {
          return {
            id: item.id || '',
            placeholder: item.placeholder || '',
            value: item.value || '',
          };
        });
      },
      error: (e) => {
        this.loggerService.error(e);
      },
    });
  }

  protected refreshDashboardsData() {
    this.isBusy = true;

    this.panelsComponents.forEach((component) => {
      component.refreshData();
    });

    timer(500).subscribe(() => (this.isBusy = false));
  }

  protected freezeGrid() {
    this.gridsterOptions!.draggable!.enabled = false;
    this.gridsterOptions!.resizable!.enabled = false;
    this.gridsterOptions.api?.optionsChanged!();
  }

  protected unfreezeGrid() {
    this.gridsterOptions!.draggable!.enabled = true;
    this.gridsterOptions!.resizable!.enabled = true;
    this.gridsterOptions.api?.optionsChanged!();
  }

  protected refreshGridsterItems() {
    this.gridsterItems = this.panels.map<GridsterItem>((panel) => {
      return { ...panel.layout, panelId: panel.id };
    });
  }

  protected toggleMode(event: any) {
    if (event.checked) {
      this.unfreezeGrid();
    } else {
      this.freezeGrid();
    }
  }

  protected setRefreshRate(seconds: number | null) {
    this.refreshRateSubscription?.unsubscribe();
    if (seconds) {
      this.refreshRateSubscription = interval(seconds * 1000).subscribe(() => {
        this.refreshDashboardsData();
      });
    }
  }

  protected saveLayout() {
    this.isSaving = true;

    const observables = this.panelsComponents.map((pc) => pc.saveLayout());

    forkJoin(observables)
      .pipe(
        finalize(() => {
          this.isSaving = false;
        }),
      )
      .subscribe({
        next: () => {
          this.loggerService.success('Saved layout');
        },
        error: () => {
          this.loggerService.error('Unable to save layout');
        },
      });
  }

  protected handleGridsterItemChange(
    item: GridsterItem,
    component: GridsterItemComponentInterface,
  ) {
    const panelId = item['panelId'];
    this.panelsComponents
      .find((pc) => pc.panelId == panelId)
      ?.updateLayout({
        x: item.x,
        y: item.y,
        cols: item.cols,
        rows: item.rows,
      });
  }

  protected updateTimeRange(timeRange: TimeRange | null) {
    if (timeRange) {
      this.panelsComponents.forEach((pc) => {
        pc.refreshTimeRange(timeRange);
      });
    }
  }
}
