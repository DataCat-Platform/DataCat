export const ALERTS_EXPLORER_URL = 'alerts';
export const ALERT_EDIT_URL = 'alerts/:alertId/edit';

export const NOTIFICATIONS_EXPLORER_URL = 'notifications';
export const NOTIFICATION_GROUP_EDIT_URL = 'notifications/groups/:groupId/edit';

export const DATA_SOURCES_EXPLORER_URL = 'data-sources';
export const DATA_SOURCES_EDIT_URL = 'data-sources/:data-source-id/edit';

export const DASHBOARDS_EXPLORER_URL = 'dashboards';
export const DASHBOARD_VIEW_URL = 'dashboards/:dashboardId';

export const PANEL_EDIT_URL = 'panels/:panelId/edit';

export const ADMIN_URL = 'admin';

export const LOGIN_URL = 'login';

export const HOME_URL = '';
export const SETTINGS_URL = 'settings';

export const LOGS_EXPLORER_URL = 'logs';

export const TRACES_EXPLORER_URL = 'traces';
export const TRACE_SPANS_EXPLORER_URL = 'traces/:traceId';

export const FORBIDDEN_URL = 'forbidden';

// -----------------------------------------------------------------------------

export const alertEditUrl = (alertId: string) => `alerts/${alertId}/edit`;

export const notificationGroupEditUrl = (groupId: string) => `notifications/groups/${groupId}/edit`;

export const dataSourceEditUrl = (dataSourceId: string) => `data-sources/${dataSourceId}/edit`;

export const logUrl = (logId: string) => `/logs/${logId}`;
export const traceUrl = (traceId: string, dataSourceName: string) => `/traces/${traceId}?dataSourceName=${dataSourceName}`;
export const dashboardUrl = (dashboardId: string) => `/dashboards/${dashboardId}`;

export const panelEditUrl = (panelId: string) => `/panels/${panelId}/edit`;
