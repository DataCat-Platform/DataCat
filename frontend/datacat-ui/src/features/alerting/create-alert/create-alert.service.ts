import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import * as paths from '../../../entities/urls';
import {
  AddAlertRequest,
  ApiService,
} from '../../../shared/services/api.service';
import { BehaviorSubject, Observable } from 'rxjs';
import {
  CreateAlertDto,
  DataSourceInfo,
  NotificationChannelInfo,
} from './create-alert.types';
import { DEFAULT_CREATE_ALERT_DTO } from './create-alert.consts';

@Injectable({
  providedIn: 'root',
})
export class CreateAlertService {
  private knownDataSourcesSubject = new BehaviorSubject<DataSourceInfo[]>([]);
  private knownNotificationChannelsSubject = new BehaviorSubject<
    NotificationChannelInfo[]
  >([]);

  public createAlertDto: CreateAlertDto;
  public knownDataSources$: Observable<DataSourceInfo[]>;
  public knownNotificationChannels$: Observable<NotificationChannelInfo[]>;

  constructor(
    private router: Router,
    private apiService: ApiService,
  ) {
    this.createAlertDto = DEFAULT_CREATE_ALERT_DTO;
    this.knownDataSources$ = this.knownDataSourcesSubject.asObservable();
    this.knownNotificationChannels$ =
      this.knownNotificationChannelsSubject.asObservable();
  }

  public create() {
    // TODO
    // const request: AddAlertRequest = {};
    // this.apiService
    //   .postApiV1AlertAdd(request)
    //   .subscribe((alertId) => this.goToAlertEdit(alertId));

    this.goToAlertEdit('0');
  }

  private goToAlertEdit(alertId: string) {
    this.router
      .navigateByUrl(paths.alertEditUrl(alertId))
      .catch((e) => console.log(e));
  }
}
