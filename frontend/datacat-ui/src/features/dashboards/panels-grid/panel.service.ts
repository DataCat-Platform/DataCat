import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { TimeSeries } from '../../../entities/dashboards/data.types';
import { Panel } from '../../../entities';

@Injectable({
  providedIn: 'root',
})
export class PanelService {
  public data$: Observable<TimeSeries[]>;
  public panel$: Observable<Panel>;

  constructor() {
    this.data$ = {} as any;
    this.panel$ = {} as any;
  }
}
