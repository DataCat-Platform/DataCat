import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class PanelService {
  private dataSubject = new BehaviorSubject<any>({});

  public data$: Observable<any>;

  constructor() {
    this.data$ = this.dataSubject.asObservable();
  }
}
