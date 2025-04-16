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
  FIRE = 'fire',
  ERROR = 'error',
};

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
