export interface BaseSettings {
  // retries: number;
};

export interface EmailSettings extends BaseSettings {
  address: string;
};

export interface WebhookSettings extends BaseSettings {
  url: string;
}

export interface TelegramSettings extends BaseSettings {
  username: string;
}

export enum NotificationChannelDriver {
  EMAIL = 'email',
  WEBHOOK = 'webhook',
  TELEGRAM = 'telegram',
};

export type NotificationChannelSettings = EmailSettings | WebhookSettings | TelegramSettings;

export type NotificationChannel = {
  id: string;
  driver: NotificationChannelDriver;
  settings: NotificationChannelSettings;
  notificationTemplateId: string;

  // lastDeliveryAt: number;
  // lastDeliveryResult: any;
};
