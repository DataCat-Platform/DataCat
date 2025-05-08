export interface BaseSettings {
};

export interface EmailSettings extends BaseSettings {
  DestinationEmail: string;
  SmtpServer: string;
  Port: number;
  PasswordPath: string;
};

export interface WebhookSettings extends BaseSettings {
  Url: string;
}

export interface TelegramSettings extends BaseSettings {
  TelegramTokenPath: string;
  ChatId: string;
}

export enum NotificationChannelDriver {
  EMAIL = 'email',
  WEBHOOK = 'webhook',
  TELEGRAM = 'telegram',
};

export type NotificationChannelSettings = EmailSettings | WebhookSettings | TelegramSettings;

export type NotificationChannel = {
  id: number;
  driver: NotificationChannelDriver;
  settings: NotificationChannelSettings;
};
