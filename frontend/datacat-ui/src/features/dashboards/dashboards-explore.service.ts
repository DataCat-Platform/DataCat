import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import {
  DashboardInSearchList,
  DashboardsFilter,
} from '../../entities/dashboard.entities';

@Injectable({
  providedIn: 'root',
})
export class DashboardsExploreService {
  private _dashboardsSubject = new BehaviorSubject<DashboardInSearchList[]>([]);

  public filter: DashboardsFilter;
  public dashboards$: Observable<DashboardInSearchList[]>;

  constructor() {
    this.dashboards$ = this._dashboardsSubject.asObservable();
    this.filter = {
      tags: [],
    };
  }

  public refreshDashboards() {
    this._dashboardsSubject.next([
      {
        id: '1',
        name: '2',
        description: 'dd',
        ownerId: 'aa',
        lastUpdatedAt: Date.now(),
        tags: [],
        isInFavorites: true,
      },
      {
        id: '2',
        name: '2',
        description: 'dd',
        ownerId: 'aa',
        lastUpdatedAt: Date.now(),
        tags: [],
        isInFavorites: false,
      },
    ]);
  }

  public removeLabelByIndex(index: number) {
    this.filter = {
      ...this.filter,
      tags: this.filter.tags.filter((_, idx) => idx !== index),
    };
  }

  public addEmptyLabel() {
    this.filter = {
      ...this.filter,
      tags: [...this.filter.tags, ''],
    };
  }
}
