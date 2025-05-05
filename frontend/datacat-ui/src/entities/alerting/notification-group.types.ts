import { NotificationChannel } from './notification-channel.types';

export type NotificationGroup = {
  id: string;
  name: string;
  notificationChannelsIds: string[];
};

export type NotificationGroupExpanded = {
  id: string;
  name: string;
  notificationChannels: NotificationChannel[];
};
