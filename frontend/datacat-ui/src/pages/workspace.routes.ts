import { Routes } from '@angular/router';
import { namespaceSelectedGuardFn } from '../shared/guards/namespace-selected.guard';
import * as urls from '../entities/urls';

export const ROUTES: Routes = [
  {
    path: urls.HOME_URL,
    loadComponent: () =>
      import('../widgets/home/home.component').then((m) => m.HomeComponent),
  },
  {
    path: '',
    canActivate: [namespaceSelectedGuardFn],
    children: [
      {
        path: urls.DASHBOARDS_EXPLORER_URL,
        loadComponent: () =>
          import(
            '../widgets/dashboards/dashboards-explore/dashboards-explore.component'
          ).then((m) => m.DashboardsExploreComponent),
      },
      {
        path: urls.DASHBOARD_URL,
        loadComponent: () =>
          import('../widgets/dashboards/dashboard/dashboard.component').then(
            (m) => m.DashboardComponent,
          ),
      },
      {
        path: urls.SETTINGS_URL,
        loadComponent: () =>
          import('../widgets/settings/settings.component').then(
            (m) => m.SettingsComponent,
          ),
      },
      {
        path: urls.ALERTS_EXPLORER_URL,
        loadComponent: () =>
          import(
            '../widgets/alerting/alerts-explorer/alerts-explorer.component'
          ).then((m) => m.AlertsExplorerComponent),
      },
      {
        path: urls.ALERT_EDIT_URL,
        loadComponent: () =>
          import('../widgets/alerting/alert-edit/alert-edit.component').then(
            (m) => m.AlertEditComponent,
          ),
      },
      {
        path: urls.NOTIFICATION_CHANNELS_URL,
        loadComponent: () =>
          import(
            '../widgets/alerting/notifications-explorer/notifications-explorer.component'
          ).then((m) => m.NotificationChannelsComponent),
      },
      {
        path: urls.ADMIN_URL,
        loadComponent: () =>
          import('../widgets/admin/admin.component').then(
            (m) => m.AdminComponent,
          ),
      },
    ],
  },
  {
    path: '**',
    redirectTo: urls.HOME_URL,
  },
];
