import {
  Alert,
  AlertStatus,
  DataSource,
  DataSourceDriver,
  NotificationChannel,
  NotificationChannelDriver,
  NotificationGroupExpanded,
  NotificationTemplate,
  NotificationTemplateSyntax,
} from '../../entities/alerting';
import { AlertsCountsByStatus } from '../../features/alerting/alerts-counts-by-status/alerts-counts-by-status.types';

export const FAKE_ALERT: Alert = {
  id: '0',
  description: 'fake alert',
  query: 'avg{something} > 0',
  executionInterval: 60,
  notificationTriggerPeriod: 120,
  notificationGroupId: '0',
  dataSourceId: '0',
  status: AlertStatus.OK,
  prevExecutionTime: Date.now(),
  nextExecutionTime: Date.now(),
};

export const FAKE_NOTIFICATION_CHANNEL: NotificationChannel = {
  id: '0',
  driver: NotificationChannelDriver.EMAIL,
  notificationTemplateId: '0',
  settings: {
    address: '',
  },
};

export const FAKE_NOTIFICATION_TEMPLATE: NotificationTemplate = {
  syntax: NotificationTemplateSyntax.MARKDOWN,
  template: 'Alert { .id } is alerting!',
};

export const FAKE_DATASOURCE: DataSource = {
  id: '0',
  name: 'fake datasource',
  driver: DataSourceDriver.PROMETHEUS,
  connectionUrl: 'http://localhost:4000/a',
};

export const FAKE_ALERTS_COUNTS_BY_STATUS: AlertsCountsByStatus = new Map<
  AlertStatus,
  number
>();
FAKE_ALERTS_COUNTS_BY_STATUS.set(AlertStatus.OK, 41);
FAKE_ALERTS_COUNTS_BY_STATUS.set(AlertStatus.ERROR, 2);
FAKE_ALERTS_COUNTS_BY_STATUS.set(AlertStatus.FIRING, 2);
FAKE_ALERTS_COUNTS_BY_STATUS.set(AlertStatus.PENDING, 1);
FAKE_ALERTS_COUNTS_BY_STATUS.set(AlertStatus.MUTED, 5);

export const getFakeId = (): string => {
  return Math.floor(Math.random() * 1000)
    .toString()
    .padStart(4, '0');
};

export const getFakeNotificationChannel = (): NotificationChannel => {
  switch (Math.floor(Math.random() * 3)) {
    case 0:
      return {
        id: getFakeId(),
        driver: NotificationChannelDriver.EMAIL,
        notificationTemplateId: getFakeId(),
        settings: {
          address: 'nvnazarov@edu.hse.ru',
        },
      };
    case 1:
      return {
        id: getFakeId(),
        driver: NotificationChannelDriver.TELEGRAM,
        notificationTemplateId: getFakeId(),
        settings: {
          username: '@nktnazarov',
        },
      };
    default:
      return {
        id: getFakeId(),
        driver: NotificationChannelDriver.WEBHOOK,
        notificationTemplateId: getFakeId(),
        settings: {
          url: 'http://123.456.789.101/webhook',
        },
      };
  }
};

export const getFakeNotificationGroupsExpanded =
  (): NotificationGroupExpanded[] => {
    const groupsCount = 3;
    const groups: NotificationGroupExpanded[] = [];

    [...Array(groupsCount)].forEach(() => {
      const group: NotificationGroupExpanded = {
        id: getFakeId(),
        name: 'fake group ' + getFakeId(),
        notificationTemplate: getFakeNotificationTemplate(),
        notificationChannels: [],
      };

      const channelsCount = Math.floor(Math.random() * 2) + 1;
      [...Array(channelsCount)].forEach(() => {
        group.notificationChannels.push(getFakeNotificationChannel());
      });

      groups.push(group);
    });

    return groups;
  };

export const getFakeNotificationTemplate = (): NotificationTemplate => {
  return {
    syntax: NotificationTemplateSyntax.MARKDOWN,
    template: 'Alert { .description } is alerting!',
  };
};
