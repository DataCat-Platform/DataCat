export interface DataSource {
  id: string;
  name: string;
  type: string;
  connectionUrl: string;
}

export interface NotificationChannel {
  id: string;
  destinationName: string;
  settings: string;
}

export enum AlertStatus {
  OK = 'ok',
  FIRING = 'firing',
  ERROR = 'error',
  PENDING = 'pending',
  MUTED = 'muted',
}

export interface Alert {
  id: string;
  description: string;
  rawQuery: string;
  status: AlertStatus;
  dataSource: DataSource;
  notificationChannel: NotificationChannel;
  previousExecutionTime: number;
  nextExecutionTime: number;
  waitTimeBeforeAlerting: number;
  repeatInterval: number;
}

export interface AlertLabel {
  key: string;
  value: string;
}

export interface AlertsExploreFilter {
  alertStatus?: AlertStatus;
  dataSource?: string;
  notificationChannel?: string;
  labels: AlertLabel[];
}
