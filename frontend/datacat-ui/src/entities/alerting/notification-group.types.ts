import { NotificationChannel } from './notification-channel.types';

export enum NotificationTemplateSyntax {
  MARKDOWN = 'markdown',
}

export type NotificationTemplate = {
  syntax: NotificationTemplateSyntax;
  template: string;
};

export type NotificationGroup = {
  id: string;
  name: string;
  notificationTemplate: NotificationTemplate;
  notificationChannelsIds: string[];
};

export type NotificationGroupExpanded = {
  id: string;
  name: string;
  notificationTemplate: NotificationTemplate;
  notificationChannels: NotificationChannel[];
};
