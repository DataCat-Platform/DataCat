import { Routes } from '@angular/router';
import { namespaceSelectedGuardFn } from '../shared/guards/namespace-selected.guard';
import * as urls from '../shared/common/urls';

export const ROUTES: Routes = [
  {
    path: urls.HOME_URL,
    loadComponent: () =>
      import('../widgets/workspace/home/home.component').then(
        (m) => m.HomeComponent,
      ),
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
          import('../processes/settings/settings.component').then(
            (m) => m.SettingsComponent,
          ),
      },
      {
        path: urls.ALERTS_EXPLORER_URL,
        loadComponent: () =>
          import('../processes/explore-alerts/alerts-explorer.component').then(
            (m) => m.AlertsExplorerComponent,
          ),
      },
      {
        path: urls.ALERT_EDIT_URL,
        loadComponent: () =>
          import('../processes/manage-alert/manage-alert.component').then(
            (m) => m.ManageAlertComponent,
          ),
      },
      {
        path: urls.ALERT_VIEW_URL,
        loadComponent: () =>
          import('../processes/view-alert/view-alert.component').then(
            (m) => m.ViewAlertComponent,
          ),
      },
      {
        path: urls.NOTIFICATION_CHANNELS_URL,
        loadComponent: () =>
          import(
            '../widgets/alerting/notifications-explorer/notifications-explorer.component'
          ).then((m) => m.NotificationChannelsComponent),
      },
    ],
  },
  {
    path: '**',
    redirectTo: urls.HOME_URL,
  },
];
