import { NotificationChannel } from './notifications.entities';
import { DataSource } from './datasource.entities';

export enum AlertStatus {
  OK = 'ok',
  FIRING = 'firing',
  ERROR = 'error',
  PENDING = 'pending',
  MUTED = 'muted',
}

export type Alert = {
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

