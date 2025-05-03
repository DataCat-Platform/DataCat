export interface BaseSettings {
  // retries: number;
};

export interface EmailSettings extends BaseSettings {
  address: string;
};

export enum WebhookMethod {
  POST = 'post',
  GET = 'get',
};

export interface WebhookSettings extends BaseSettings {
  url: string;
  method: WebhookMethod;
}

export interface TelegramSettings extends BaseSettings {
  username: string;
}

export enum NotificationChannelDriver {
  EMAIL = 'email',
  WEBHOOK = 'webhook',
  TELEGRAM = 'telegram',
};

export type NotificationChannelSettings = EmailSettings;

export type NotificationChannel = {
  id: string;
  driver: NotificationChannelDriver;
  settings: NotificationChannelSettings;
  notificationTemplateId: string;

  // lastDeliveryAt: number;
  // lastDeliveryResult: any;
};
