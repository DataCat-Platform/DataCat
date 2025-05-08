export enum AlertStatus {
  OK = 'ok',
  FIRING = 'firing',
  ERROR = 'error',
  PENDING = 'pending',
  MUTED = 'muted',
}

export type Alert = {
  id: string;
  template: string;
  description: string;
  query: string;
  status: string;
  dataSourceId: string;
  notificationGroupId: string;
  prevExecutionTime: number;
  nextExecutionTime: number;
  notificationTriggerPeriod: number;
  executionInterval: number;
};
