export enum NotificationTemplateSyntax {
  MARKDOWN = 'markdown',
}

export type NotificationTemplate = {
  id: string;
  name: string;
  syntax: NotificationTemplateSyntax;
  template: string;
};
