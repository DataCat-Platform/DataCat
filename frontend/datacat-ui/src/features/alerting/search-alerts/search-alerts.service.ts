import { Injectable } from '@angular/core';
import { Alert, AlertStatus } from '../../../entities/alerting/alerting.types';

@Injectable({
  providedIn: 'root',
})
export class SearchAlertsService {
  private _alerts: Alert[] = [];

  get alerts(): Alert[] {
    return this._alerts;
  }

  searchAlerts(): void {
    this._alerts = [
      {
        id: '1',
        description: 'RT is too big',
        rawQuery: '',
        status: AlertStatus.OK,
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
        previousExecutionTime: Date.now(),
        nextExecutionTime: Date.now(),
        waitTimeBeforeAlerting: 10,
        repeatInterval: 10,
      },
      {
        id: '2',
        description: 'ER is too big',
        rawQuery: '',
        status: AlertStatus.FIRE,
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
        previousExecutionTime: Date.now(),
        nextExecutionTime: Date.now(),
        waitTimeBeforeAlerting: 10,
        repeatInterval: 10,
      },
      {
        id: '3',
        description: '...',
        rawQuery: '',
        status: AlertStatus.ERROR,
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
        previousExecutionTime: Date.now(),
        nextExecutionTime: Date.now(),
        waitTimeBeforeAlerting: 10,
        repeatInterval: 10,
      },
    ];
  }
}
