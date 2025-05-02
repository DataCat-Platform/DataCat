import { Injectable } from '@angular/core';
import {
  AlertInList,
  AlertsCountsByStatus,
  AlertsFilter,
  DataSourceInfo,
  NotificationChannelInfo,
} from './alerts-explorer.types';
import { BehaviorSubject, Observable } from 'rxjs';
import {
  DEFAULT_ALERTS_FILTER,
  getDefaultAlertsCountsByStatus,
} from './alerts-explorer.consts';
import { ApiService } from '../../../shared/services/api.service';
import { MockService } from '../../../shared/mock/mock.service';
import { Router } from '@angular/router';
import * as urls from '../../../entities/urls';

@Injectable({
  providedIn: 'root',
})
export class AlertsExplorerService {
  private alertsSubject = new BehaviorSubject<AlertInList[]>([]);
  private alertsCountsByStatusSubject: BehaviorSubject<AlertsCountsByStatus>;
  private knownDataSourcesSubject = new BehaviorSubject<DataSourceInfo[]>([]);
  private knownNotificationChannelsSubject = new BehaviorSubject<
    NotificationChannelInfo[]
  >([]);

  public readonly alerts$: Observable<AlertInList[]>;
  public readonly alertsFilter: AlertsFilter;
  public readonly alertsCountsByState$: Observable<AlertsCountsByStatus>;
  public readonly knownDataSources$: Observable<DataSourceInfo[]>;
  public readonly knownNotificationChannels$: Observable<
    NotificationChannelInfo[]
  >;

  constructor(
    private apiService: ApiService,
    private mock: MockService,
    private router: Router,
  ) {
    this.alerts$ = this.alertsSubject.asObservable();
    this.alertsFilter = DEFAULT_ALERTS_FILTER;
    this.knownDataSources$ = this.knownDataSourcesSubject.asObservable();
    this.knownNotificationChannels$ =
      this.knownNotificationChannelsSubject.asObservable();
    this.alertsCountsByStatusSubject =
      new BehaviorSubject<AlertsCountsByStatus>(
        getDefaultAlertsCountsByStatus(),
      );
    this.alertsCountsByState$ = this.alertsCountsByStatusSubject.asObservable();
  }

  public addTag(tag: string) {
    if (this.alertsFilter.tags.indexOf(tag) == -1) {
      this.alertsFilter.tags.push(tag);
    }
  }

  public removeTag(tag: string) {
    this.alertsFilter.tags = this.alertsFilter.tags.filter((t) => t !== tag);
  }

  public goToAlertEditPage(alertId: string) {
    this.router.navigateByUrl(urls.alertEditUrl(alertId));
  }

  public refreshAlertsByStatusCounts() {
    // TODO: call API
  }

  public refreshAlerts() {
    // TODO: call API
    this.alertsSubject.next(
      new Array(this.alertsFilter.pageSize).map((_) =>
        this.mock.getRandomAlertInList(),
      ),
    );
  }
}
