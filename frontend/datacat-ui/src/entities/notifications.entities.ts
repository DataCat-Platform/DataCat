export type NotificationChannel = {
  id: string;
  templateId: string;
  channelType: NotificationChannelType;
  channelSettings: EmailSettings | TelegramSettings | WebhookSettings;
};

export enum NotificationTemplateType {
  MARKDOWN = 'markdown',
}

export type NotificationTemplate = {
  id: string;
  type: NotificationTemplateType;
  template: string;
};

export enum NotificationChannelType {
  EMAIL,
  TELEGRAM,
  WEBHOOK,
}

export type ChannelSettings = EmailSettings | TelegramSettings | WebhookSettings;

export type EmailSettings = {
  receiverAddress: string;
};

export type TelegramSettings = {
  receiverUsername: string;
};

export type WebhookSettings = {
  method: 'GET' | 'POST';
  remoteUrl: string;
};
