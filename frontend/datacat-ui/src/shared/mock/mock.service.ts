import { Injectable } from '@angular/core';
import {
  AlertStatus,
  DataSource,
  DataSourceType,
  NotificationChannel,
  NotificationChannelType,
  NotificationTemplateType,
} from '../../entities';
import { NotificationTemplate } from '../../features/alerting/notifications-explorer/notifications-explorer.types';
import { Alert } from '../../entities/alerting';

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

  public getRandomAlert(): Alert {
    const statuses = Object.values(AlertStatus);
    return {
      id: 'id',
      description: 'description',
      query: 'query',
      status: statuses[Math.floor(Math.random() * statuses.length)],
      dataSourceId: '0',
      notificationGroupId: '0',
      prevExecutionTime: Date.now(),
      nextExecutionTime: Date.now(),
      notificationTriggerPeriod: 60,
      executionInterval: 10,
    };
  }
}
