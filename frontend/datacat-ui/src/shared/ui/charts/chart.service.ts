import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { TimeSeries } from './chart.types';
import { ApiService } from '../../services/datacat-generated-client';
import { VisualizationType } from '../../../entities';

@Injectable({
  providedIn: 'root',
})
export class ChartService {
  public readonly type$: Observable<VisualizationType>;
  public readonly data$: Observable<TimeSeries[]>;
  public readonly style$: Observable<any>;

  public query?: string;
  public dataSourceName?: string;

  private typeSubject = new BehaviorSubject<VisualizationType>(
    VisualizationType.LINE,
  );
  private dataSubject = new BehaviorSubject<any>({});
  private styleSubject = new BehaviorSubject<any>({});

  constructor(private api: ApiService) {
    this.type$ = this.typeSubject.asObservable();
    this.data$ = this.dataSubject.asObservable();
    this.style$ = this.styleSubject.asObservable();
  }

  public updateStyle(style: any): void {
    this.styleSubject.next({
      ...this.styleSubject.value,
      ...style,
    });
  }

  public showTimeRange(from: Date, to: Date, step: string): void {}

  public addPointAt(date: Date): void {}

  private loadTimeRange() {
    this.api;
  }
}
