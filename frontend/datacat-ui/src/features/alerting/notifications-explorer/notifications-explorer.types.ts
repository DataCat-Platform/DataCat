import { ChannelSettings, NotificationChannelType } from '../../../entities';
import { NotificationTemplateType } from '../../../entities';

export type NotificationChannel = {
  type: NotificationChannelType;
  channelSettings: ChannelSettings;
  lastDeliveryAttempt?: number;
};

export type NotificationTemplate = {
  id: string;
  type: NotificationTemplateType;
  name: string;
  template: string;
};

export type NotificationChannelsGroup = {
  id: string;
  name: string;
  channels: NotificationChannel[];
};
