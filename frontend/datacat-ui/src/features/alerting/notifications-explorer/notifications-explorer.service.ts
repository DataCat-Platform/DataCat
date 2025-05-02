import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import {
  NotificationChannel,
  NotificationChannelsGroup,
  NotificationTemplate,
} from './notifications-explorer.types';
import { MockService } from '../../../shared/mock/mock.service';

@Injectable({
  providedIn: 'root',
})
export class NotificationsExplorerService {
  private channelsGroupsSubject = new BehaviorSubject<
    NotificationChannelsGroup[]
  >([]);
  private notificationTemplatesSubject = new BehaviorSubject<
    NotificationTemplate[]
  >([]);

  public readonly channelsGroups$: Observable<NotificationChannelsGroup[]>;
  public readonly notificationTemplates$: Observable<NotificationTemplate[]>;

  constructor(private mock: MockService) {
    this.channelsGroups$ = this.channelsGroupsSubject.asObservable();
    this.notificationTemplates$ =
      this.notificationTemplatesSubject.asObservable();
  }

  public refreshChannelsGroups() {
    // TODO: call API
    const channelsGroups = [...Array(3)].map<NotificationChannelsGroup>((_) => {
      const channels = [
        ...Array(1 + Math.floor(Math.random() * 2)),
      ].map<NotificationChannel>((_) => {
        const m = this.mock.getRandomNotificationChannel();
        return {
          id: m.id,
          type: m.channelType,
          channelSettings: m.channelSettings,
        };
      });
      return {
        id: Math.floor(Math.random() * 1000).toString(),
        name: 'group name ' + Math.floor(Math.random() * 1000),
        channels: channels,
      };
    });
    this.channelsGroupsSubject.next(channelsGroups);
  }

  public refreshNotificationTemplates() {
    const templates = [...Array(4)].map(this.mock.getRandomNotificationTemplate);
    this.notificationTemplatesSubject.next(templates);
  }

  public test(channel: NotificationChannel) {
    // TODO
  }

  public deleteChannelsGroup(channelsGroup: NotificationChannelsGroup) {
    // TODO
  }
}
