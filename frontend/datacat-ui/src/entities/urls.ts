import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { MenuItem } from 'primeng/api';

export const ALERTS_EXPLORER_URL = 'alerts/explorer';
export const ALERT_CREATE_URL = 'alerts/create';
export const ALERT_EDIT_URL = 'alerts/:alertId';
export const DASHBOARDS_EXPLORER_URL = 'dashboards/explorer';
export const DASHBOARD_URL = 'dashboards/:dashboardId';
export const PANEL_EDIT_URL = 'panels/:panelId';
export const ADMIN_URL = 'admin';
export const LOGIN_URL = 'login';
export const HOME_URL = '';
export const SETTINGS_URL = 'settings';
export const LOGS_EXPLORER_URL = 'logs/explorer';
export const TRACES_EXPLORER_URL = 'traces/explorer';
export const NOTIFICATION_CHANNELS_URL = 'notifications';

export const alertEditUrl = (alertId: string) => `alerts/${alertId}`;

export const logUrl = (logId: string) => `/logs/${logId}`;

export const traceUrl = (traceId: string) => `/traces/${traceId}`;

export const dashboardUrl = (dashboardId: string) =>
  `/dashboards/${dashboardId}`;

export const panelEditUrl = (panelId: string) => `/panels/${panelId}`;

export function currentUrlToActivityPath(): MenuItem[] {
  const router = inject(Router);
  return [];
}
