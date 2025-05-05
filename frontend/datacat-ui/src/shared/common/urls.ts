export const ALERTS_EXPLORER_URL = 'alerts';
export const ALERT_EDIT_URL = 'alerts/:alertId/edit';
export const ALERT_VIEW_URL = 'alerts/:alertId/view';
export const NOTIFICATIONS_EXPLORER_URL = 'notifications';
export const NOTIFICATION_GROUP_EDIT_URL =
  'notifications/groups/:groupId/edit';
export const NOTIFICATION_TEMPLATE_EDIT_URL =
  'notifications/templates/:templateId/edit';
export const DASHBOARDS_EXPLORER_URL = 'dashboards';
export const DASHBOARD_VIEW_URL = 'dashboards/:dashboardId';
export const PANEL_EDIT_URL = 'panels/:panelId/edit';
export const ADMIN_URL = 'admin';
export const LOGIN_URL = 'login';
export const HOME_URL = '';
export const SETTINGS_URL = 'settings';
export const LOGS_EXPLORER_URL = 'logs';
export const TRACES_EXPLORER_URL = 'traces';

export const alertEditUrl = (alertId: string) => `alerts/${alertId}/edit`;
export const alertViewUrl = (alertId: string) => `alerts/${alertId}/view`;
export const notificationTemplateEditUrl = (templateId: string) =>
  `notifications/templates/${templateId}/edit`;
export const notificationGroupEditUrl = (groupId: string) =>
  `notifications/groups/${groupId}/edit`;
export const logUrl = (logId: string) => `/logs/${logId}`;
export const traceUrl = (traceId: string) => `/traces/${traceId}`;
export const dashboardUrl = (dashboardId: string) =>
  `/dashboards/${dashboardId}`;
export const panelEditUrl = (panelId: string) => `/panels/${panelId}`;
