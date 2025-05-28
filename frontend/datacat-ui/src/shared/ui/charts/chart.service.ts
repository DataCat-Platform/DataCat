import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, Subscription } from 'rxjs';
import { DataPoint, TimeSeries } from './chart.types';
import { ApiService } from '../../services/datacat-generated-client';
import { VisualizationType } from '../../../entities';
import { ThemeSelectionService } from '../../../features/appearence/select-theme/select-theme.service';

@Injectable({
  providedIn: 'root',
})
export class ChartService {
  public readonly type$: Observable<VisualizationType>;
  public readonly data$: Observable<TimeSeries[]>;
  public readonly style$: Observable<any>;
  public readonly error$: Observable<boolean>;

  public query?: string;
  public dataSourceName?: string;

  private typeSubject = new BehaviorSubject<VisualizationType>(
    VisualizationType.LINE,
  );
  private dataSubject = new BehaviorSubject<any>([]);
  private styleSubject = new BehaviorSubject<any>({});
  private errorSubject = new BehaviorSubject<boolean>(false);

  public get type(): VisualizationType {
    return this.typeSubject.value;
  }

  public get data(): any {
    return this.dataSubject.value;
  }

  public get style(): any {
    return this.styleSubject.value;
  }

  private loadTimeRangeSubscription?: Subscription;

  constructor(private api: ApiService) {
    this.type$ = this.typeSubject.asObservable();
    this.data$ = this.dataSubject.asObservable();
    this.style$ = this.styleSubject.asObservable();
    this.error$ = this.errorSubject.asObservable();
  }

  public setType(type: VisualizationType): void {
    this.typeSubject.next(type);
  }

  public updateStyle(style: any): void {
    this.styleSubject.next({
      ...this.styleSubject.value,
      ...style,
    });
  }

  public setQuery(query: string): void {
    this.query = query;
  }

  public setDataSourceName(name: string): void {
    this.dataSourceName = name;
  }

  public loadTimeRange(from: Date, to: Date, step: string): void {
    if (!this.dataSourceName || !this.query) return;

    this.loadTimeRangeSubscription?.unsubscribe();
    this.errorSubject.next(false);
    this.loadTimeRangeSubscription = this.api
      .getApiV1MetricsQueryRange(
        this.dataSourceName,
        this.query,
        'undefined' as any,
        null,
        from,
        to,
        step,
      )
      .subscribe({
        next: (data) => {
          const timeSeries =
            data.map<TimeSeries>((ts) => {
              return {
                name: ts.metricName!,
                labels: ts.labels!,
                points:
                  ts.points?.map<DataPoint>((p) => {
                    return {
                      value: p.value!,
                      timestamp: p.timestamp!,
                      labels: p.labels!,
                    };
                  }) || [],
              };
            }) || [];
          this.dataSubject.next(timeSeries);
        },
        error: () => {
          this.errorSubject.next(true);
        },
      });
  }

  public addPointAt(date: Date): void {}
}
