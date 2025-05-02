import { Injectable } from '@angular/core';
import {
  AlertStatus,
  DataSource,
  DataSourceType,
  NotificationChannel,
  NotificationChannelType,
  NotificationTemplateType,
} from '../../entities';
import { AlertInList } from '../../features/alerting/alerts-explorer/alerts-explorer.types';
import { NotificationTemplate } from '../../features/alerting/notifications-explorer/notifications-explorer.types';

@Injectable({
  providedIn: 'root',
})
export class MockService {
  public getRandomNotificationChannel(): NotificationChannel {
    const id = Math.floor(Math.random() * 1000).toString();
    const templateId = Math.floor(Math.random() * 1000).toString();

    switch (Math.floor(Math.random() * 3)) {
      case 0: {
        return {
          id: id,
          channelType: NotificationChannelType.TELEGRAM,
          channelSettings: {
            receiverUsername: '@nvnazarov',
          },
          templateId: templateId,
        };
      }
      case 1: {
        return {
          id: id,
          channelType: NotificationChannelType.WEBHOOK,
          channelSettings: {
            method: Math.floor(Math.random() * 2) == 0 ? 'POST' : 'GET',
            remoteUrl: 'https://...',
          },
          templateId: templateId,
        };
      }
      default: {
        return {
          id: id,
          channelType: NotificationChannelType.EMAIL,
          channelSettings: {
            receiverAddress: 'nvnazarov@edu.hse.ru',
          },
          templateId: templateId,
        };
      }
    }
  }

  public getRandomNotificationTemplate(): NotificationTemplate {
    return {
      id: Math.round(Math.random() * 1000).toString(),
      type: NotificationTemplateType.MARKDOWN,
      name: 'template name',
      template: 'Alert { .id }',
    };
  }

  public getRandomDataSource(): DataSource {
    return {
      id: Math.floor(Math.random() * 1000).toString(),
      name: 'name',
      type: DataSourceType.PROMETHEUS,
      connectionUrl: 'http://...',
    };
  }

  public getRandomAlertInList(): AlertInList {
    const statusIndex = Math.floor(
      Math.random() * Object.values(AlertStatus).length,
    );
    return {
      id: Math.floor(Math.random() * 1000).toString(),
      description: 'Description',
      query: 'Query',
      status: Object.values(AlertStatus)[statusIndex],
      dataSource: this.getRandomDataSource(),
      notificationChannel: this.getRandomNotificationChannel(),
      previousExecutionTime: Math.floor(Math.random() * 1000),
      nextExecutionTime: Math.floor(Math.random() * 1000),
      notificationTriggerPeriod: Math.floor(Math.random() * 1000),
      executionInterval: Math.floor(Math.random() * 1000),
    };
  }
}
