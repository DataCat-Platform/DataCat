export enum AlertStatus {
  OK = 'ok',
  FIRING = 'firing',
  ERROR = 'error',
  MUTED = 'muted',
}

export type Alert = {
  id: string;
  template: string;
  description: string;
  query: string;
  status: AlertStatus;
  dataSourceId: string;
  notificationGroupId: string;
  prevExecutionTime: number;
  nextExecutionTime: number;
  notificationTriggerPeriod: string;
  executionInterval: string;
};
