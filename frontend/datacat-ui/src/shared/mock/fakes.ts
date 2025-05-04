import {
  Alert,
  AlertStatus,
  DataSource,
  DataSourceDriver,
  NotificationChannel,
  NotificationChannelDriver,
  NotificationGroup,
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

export const FAKE_NOTIFICATION_GROUP: NotificationGroup = {
  id: '0',
  name: 'fake group',
  notificationChannelsIds: ['0'],
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
  id: '0',
  name: 'fake template',
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
