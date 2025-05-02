import {
  AlertStatus,
  DataSource,
  NotificationChannel,
} from '../../../entities';

export type DataSourceInfo = {
  id: string;
  name: string;
}

export type NotificationChannelInfo = {
  id: string;
  name: string;
}

export type AlertsFilter = {
  alertStatus?: AlertStatus;
  dataSource?: string;
  notificationChannel?: string;
  tags: string[];
  page: number;
  pageSize: number;
};

export type AlertInList = {
  id: string;
  description: string;
  query: string;
  status: AlertStatus;
  dataSource: DataSource;
  notificationChannel: NotificationChannel;
  previousExecutionTime: number;
  nextExecutionTime: number;
  notificationTriggerPeriod: number;
  executionInterval: number;
};

export type AlertsCountsByStatus = Map<AlertStatus, number>;
