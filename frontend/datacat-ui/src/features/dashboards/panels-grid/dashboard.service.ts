import { Injectable } from '@angular/core';
import {
  BehaviorSubject,
  finalize,
  forkJoin,
  Observable,
  ObservableLike,
} from 'rxjs';
import {
  Dashboard,
  DashboardVariable,
  decodeLayout,
  mapGetPanelResponeToPanel,
  mapVariableResponseToDashboardVariable,
  Panel,
  serializeLayout,
  serializeVisualizationSettings,
  serializeVisualizationType,
} from '../../../entities';
import { TimeRange } from '../../../entities/dashboards/etc.types';
import { ApiService } from '../../../shared/services/datacat-generated-client';
import { ToastLoggerService } from '../../../shared/services/toast-logger.service';

@Injectable({
  providedIn: 'root',
})
export class DashboardService {
  public dashboard$: Observable<Dashboard | null>;
  public timeRange$: Observable<TimeRange | null>;
  public refreshCall$: Observable<void>;
  public panels$: Observable<Panel[] | null>;
  public isBusy$: Observable<boolean>;
  public variables$: Observable<DashboardVariable[] | null>;

  private dashboardSubject = new BehaviorSubject<Dashboard | null>(null);
  private timeRangeSubject = new BehaviorSubject<TimeRange | null>(null);
  private panelsSubject = new BehaviorSubject<Panel[] | null>(null);
  private isBusySubject = new BehaviorSubject<boolean>(false);
  private variablesSubject = new BehaviorSubject<DashboardVariable[] | null>(
    null,
  );

  private panels?: Panel[];
  private dashboard?: Dashboard;

  constructor(
    private api: ApiService,
    private logger: ToastLoggerService,
  ) {
    this.dashboard$ = this.dashboardSubject.asObservable();
    this.timeRange$ = this.timeRangeSubject.asObservable();
    this.refreshCall$ = {} as any;
    this.panels$ = this.panelsSubject.asObservable();
    this.isBusy$ = this.isBusySubject.asObservable();
    this.variables$ = this.variablesSubject.asObservable();
  }

  public set dashboardId(id: string) {
    if (this.dashboard?.id === id) return;
    this.refreshDashboardPanelsById(id);
  }

  public set timeRange(tr: TimeRange) {
    this.timeRangeSubject.next(tr);
  }

  public savePanelsLayout() {
    this.isBusySubject.next(true);
    const requests = this.panels!.map((p) => {
      const request = {
        title: p.title,
        type: serializeVisualizationType(p.visualizationType!),
        rawQuery: p.query,
        dataSourceId: p.dataSource!.id,
        layout: serializeLayout(p.layout!),
        styleConfiguration: serializeVisualizationSettings(
          p.visualizationSettings!,
        ),
      } as any;
      return this.api.putApiV1PanelUpdate(p.id, request);
    });

    forkJoin(requests)
      .pipe(finalize(() => this.isBusySubject.next(false)))
      .subscribe({
        error: () => {
          this.logger.error('Cannot save full layout');
        },
      });
  }

  public refreshDashboardVariables() {
    if (!this.dashboard) return;

    this.api.getApiV1VariablesDashboard(this.dashboard.id).subscribe({
      next: (data) => {
        const variables = data.map(mapVariableResponseToDashboardVariable);
        this.variablesSubject.next(variables);
      },
      error: () => {
        this.logger.error('Unable to update variables');
      },
    });
  }

  public refreshDashboardOnly() {
    if (!this.dashboard) return;
    this.api
      .getApiV1DashboardFull(this.dashboard.id)
      .pipe(finalize(() => this.isBusySubject.next(false)))
      .subscribe({
        next: (data) => {
          const panels = data.panels!.map<Panel>((p) => {
            return {
              id: p.id!,
              title: p.title!,
              query: p.query!,
              layout: decodeLayout(p.layout!),
            };
          });
          this.dashboard = {
            id: data.id!,
            name: data.name!,
            description: data.description!,
            panels,
            createdAt: data.createdAt!,
            lastUpdatedAt: data.updatedAt!,
          };
          this.dashboardSubject.next(this.dashboard!);
        },
        error: () => {},
      });
  }

  public refreshDashboardPanels() {
    if (!this.dashboard) return;
    this.refreshDashboardPanelsById(this.dashboard.id);
  }

  private refreshDashboardPanelsById(id: string) {
    this.isBusySubject.next(true);
    this.api
      .getApiV1DashboardFull(id)
      .pipe(finalize(() => this.isBusySubject.next(false)))
      .subscribe({
        next: (data) => {
          const panels = data.panels!.map<Panel>((p) => {
            return {
              id: p.id!,
              title: p.title!,
              query: p.query!,
              layout: decodeLayout(p.layout!),
            };
          });
          this.dashboard = {
            id: data.id!,
            name: data.name!,
            description: data.description!,
            panels,
            createdAt: data.createdAt!,
            lastUpdatedAt: data.updatedAt!,
          };
          this.dashboardSubject.next(this.dashboard!);

          const requests = panels.map((p) => this.api.getApiV1Panel(p.id));
          forkJoin(requests).subscribe({
            next: (data) => {
              this.panels = data.map<Panel>(mapGetPanelResponeToPanel);
              this.panelsSubject.next(this.panels);
            },
            error: () => {
              this.logger.error('Cannot load panel data');
            },
          });
        },
        error: () => {
          this.logger.error('Cannot load dashboard');
        },
      });
  }
}
