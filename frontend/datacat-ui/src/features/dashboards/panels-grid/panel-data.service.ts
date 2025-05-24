import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { DataPoint, TimeSeries } from '../../../entities/dashboards/data.types';
import { Panel } from '../../../entities';
import { TimeRange } from '../../../entities/dashboards/etc.types';
import { ApiService } from '../../../shared/services/datacat-generated-client';

@Injectable({
  providedIn: 'root',
})
export class PanelDataService {
  public data$: Observable<TimeSeries[] | null>;
  public error$: Observable<boolean>;

  private dataSubject = new BehaviorSubject<TimeSeries[] | null>(null);
  private errorSubject = new BehaviorSubject<boolean>(false);

  private data?: TimeSeries[];
  private _panel?: Panel;

  public set panel(p: Panel | undefined) {
    this._panel = p;
  }

  constructor(private api: ApiService) {
    this.data$ = this.dataSubject.asObservable();
    this.error$ = this.errorSubject.asObservable();
  }

  public loadTimeRange(tr: TimeRange): void {
    if (!this._panel) return;

    console.log(tr, this._panel.query, this._panel.dataSource!.name);

    this.errorSubject.next(false);
    this.api
      .getApiV1MetricsQueryRange(
        this._panel.dataSource!.name,
        this._panel.query,
        'undefined' as any,
        null,
        tr.from,
        tr.to,
        tr.step,
      )
      .subscribe({
        next: (data) => {
          console.log(data);
          if (data.length !== 0) {
            this.data =
              data.map<TimeSeries>((ts) => {
                return {
                  metric: ts.metricName,
                  labels: ts.labels,
                  dataPoints:
                    ts.points?.map<DataPoint>((p) => {
                      return {
                        value: p.value!,
                        timestamp: p.timestamp!,
                      };
                    }) || [],
                };
              }) || [];
          } else {
            this.data = [];
          }
          this.dataSubject.next(this.data);
        },
        error: () => {
          this.errorSubject.next(true);
        },
      });
  }
}
