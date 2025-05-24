import { Component } from '@angular/core';
import {
  Dashboard,
  DashboardVariable,
  Panel,
  PanelType,
} from '../../../entities';
import { ButtonModule } from 'primeng/button';
import { SelectModule } from 'primeng/select';
import { InputGroupModule } from 'primeng/inputgroup';
import { InputGroupAddonModule } from 'primeng/inputgroupaddon';
import { ToggleButtonModule } from 'primeng/togglebutton';
import { Subscription } from 'rxjs';
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
import { REFRESH_RATE_OPTIONS, RefreshRateOption } from '.';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { AddVariableButtonComponent } from '../add-variable';
import { DeleteVariableButtonComponent } from '../delete-variable';
import { DatePickerModule } from 'primeng/datepicker';
import { TimeRangeSelectComponent } from '../../../shared/ui/time-range-select';
import { DashboardService } from './dashboard.service';
import { DEFAULT_TIME_RANGE } from './panels-grid.consts';
import { TimeRange } from '../../../entities/dashboards/etc.types';

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
  protected refreshRateOptions: RefreshRateOption[] = REFRESH_RATE_OPTIONS;
  protected refreshRateControl = new FormControl<number | null>(null);
  protected refreshRateSubscription?: Subscription;
  protected timeRangeControl = new FormControl<TimeRange>(DEFAULT_TIME_RANGE);

  protected dashboard: Dashboard | null = null;
  protected variables: DashboardVariable[] | null = null;
  protected panels: Panel[] | null = null;
  protected panelTypes: PanelType[] = [];
  protected isBusy = false;

  protected gridsterItems: GridsterItem[] | null = null;
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

  constructor(private dashboardService: DashboardService) {
    this.freezeGrid();
    // this.refreshRateControl.valueChanges.subscribe((seconds) =>
    //   this.setRefreshRate(seconds),
    // );
    this.timeRangeControl.valueChanges.subscribe((timeRange) => {
      this.dashboardService.timeRange = timeRange!;
    });
    this.dashboardService.panels$.subscribe((v) => {
      this.panels = v;
      this.refreshGridsterItems();
    });
    this.dashboardService.isBusy$.subscribe((v) => (this.isBusy = v));
    this.dashboardService.dashboard$.subscribe((v) => (this.dashboard = v));
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

  protected refreshDashboardVariables() {
    this.dashboardService.refreshDashboardVariables();
  }

  protected refreshDashboardPanels() {
    this.dashboardService.refreshDashboardPanels();
  }

  protected savePanelsLayout() {
    this.dashboardService.savePanelsLayout();
  }

  protected refreshGridsterItems() {
    this.gridsterItems =
      this.panels?.map<GridsterItem>((panel) => {
        return { ...panel.layout, panelId: panel.id };
      }) || null;
  }

  protected toggleMode(event: any) {
    if (event.checked) {
      this.unfreezeGrid();
    } else {
      this.freezeGrid();
    }
  }

  protected handleGridsterItemChange(
    item: GridsterItem,
    component: GridsterItemComponentInterface,
  ) {
    const panelId = item['panelId'];
  }
}
