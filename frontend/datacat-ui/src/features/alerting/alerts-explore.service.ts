import { Injectable } from '@angular/core';
import {
  Alert,
  AlertStatus,
  AlertsExploreFilter,
} from '../../entities/alerting.entities';
import { BehaviorSubject, Observable } from 'rxjs';
import { ApiService } from '../../shared/services/api.service';

@Injectable({
  providedIn: 'root',
})
export class AlertsExploreService {
  private _alertsCountByState = new Map<AlertStatus, number>();
  private _filter: AlertsExploreFilter = { labels: [] };
  private _knownNotificationChannelsSubject = new BehaviorSubject<string[]>([]);
  private _knownDataSourcesSubject = new BehaviorSubject<string[]>([]);
  private _alertsSubject = new BehaviorSubject<Alert[]>([]);

  public alerts$: Observable<Alert[]>;
  public knownDataSources$: Observable<string[]>;
  public knowsNotificationChannels$: Observable<string[]>;

  public get filter(): AlertsExploreFilter {
    return this._filter;
  }

  public get alertsCountByState(): Map<AlertStatus, number> {
    return this._alertsCountByState;
  }

  constructor(private _apiService: ApiService) {
    this.alerts$ = this._alertsSubject.asObservable();
    this.knownDataSources$ =
      this._knownNotificationChannelsSubject.asObservable();
    this.knowsNotificationChannels$ =
      this._knownNotificationChannelsSubject.asObservable();
    Object.values(AlertStatus).forEach((v) =>
      this._alertsCountByState.set(v, Math.floor(Math.random() * 40)),
    );
    this._knownDataSourcesSubject.next(['Example data source']);
    this._knownNotificationChannelsSubject.next([
      'Example notification channel',
    ]);
  }

  public addEmptyAlertLabel() {
    this._filter = {
      ...this._filter,
      labels: [
        ...(this._filter.labels || []),
        {
          key: '',
          value: '',
        },
      ],
    };
  }

  public removeLabelByIndex(index: number) {
    this._filter = {
      ...this._filter,
      labels: this._filter.labels?.filter((_, idx) => idx !== index),
    };
  }

  public searchAlerts(): void {
    let newAlerts: Alert[] = [];
    for (let i = 1; i < 30; ++i) {
      const idx = Math.floor(Math.random() * Object.keys(AlertStatus).length);
      const randomStatus = Object.values(AlertStatus)[idx] as AlertStatus;
      newAlerts.push({
        id: i.toString(),
        description: 'RT is too big',
        rawQuery: '',
        status: randomStatus,
        dataSource: {
          id: '1',
          name: 'prometheus-central',
          type: 'Prometheus',
          connectionUrl: 'http://usr:pwd@127.0.0.1',
        },
        notificationChannel: {
          id: '1',
          destinationName: '?',
          settings: '?',
        },
        previousExecutionTime: Date.now() + Math.random() * 3600000,
        nextExecutionTime: Date.now() + Math.random() * 3600000,
        waitTimeBeforeAlerting: 10,
        repeatInterval: 10,
      });
    }
    this._alertsSubject.next(newAlerts);
  }
}
