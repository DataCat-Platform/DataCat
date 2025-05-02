import { CreateAlertDto } from './create-alert.types';

export const DEFAULT_CREATE_ALERT_DTO: CreateAlertDto = {
  description: '',
  query: '',
  notificationTriggerPeriod: 5,
  executionInterval: 5,
};
