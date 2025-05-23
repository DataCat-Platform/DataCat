export enum AlertStatus {
  OK = 'Ok',
  FIRING = 'Fire',
  ERROR = 'Error',
  MUTED = 'Muted',
}

export type Alert = {
  id: string;
  template: string;
  description: string;
  query: string;
  status: AlertStatus;
  dataSourceId: string;
  notificationGroupId: string;
  prevExecutionTime?: Date;
  nextExecutionTime?: Date;
  notificationTriggerPeriod: string;
  executionInterval: string;
};

export type AlertExpanded = {};
