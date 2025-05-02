export type DataSourceInfo = {
  id: string;
  name: string;
}

export type NotificationChannelInfo = {
  id: string;
  name: string;
}

export type CreateAlertDto = {
  description: string;
  query: string;
  dataSourceId?: string;
  notificationChannelId?: string;
  notificationTriggerPeriod: number;
  executionInterval: number;
};
