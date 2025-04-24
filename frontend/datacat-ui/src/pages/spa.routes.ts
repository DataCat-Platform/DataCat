import { Routes } from '@angular/router';

export const ROUTES: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('../widgets/home/home-page.component').then(
        (m) => m.HomePageComponent,
      ),
  },
  {
    path: 'dashboards',
    loadComponent: () =>
      import(
        '../widgets/dashboards/dashboard-list-widget/dashboard-list.component'
      ).then((m) => m.DashboardListComponent),
  },
  {
    path: 'dashboards/:id',
    loadComponent: () =>
      import('../widgets/dashboards/dashboard.component').then(
        (m) => m.DashboardComponent,
      ),
  },
  {
    path: 'settings',
    loadComponent: () =>
      import('../widgets/settings/settings.component').then(
        (m) => m.SettingsPageComponent,
      ),
  },
  {
    path: 'alerting/alerts/:id',
    loadComponent: () =>
      import('../widgets/alerting/alert-edit/alert-edit.component').then(
        (m) => m.AlertEditComponent,
      ),
  },
  {
    path: 'alerting/alerts',
    loadComponent: () =>
      import(
        '../widgets/alerting/alerts-explore/alerts-explore.component'
      ).then((m) => m.AlertsSearchPageComponent),
  },
  {
    path: '**',
    redirectTo: '',
  },
];
