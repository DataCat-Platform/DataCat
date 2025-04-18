import { Routes } from '@angular/router';

export const ROUTES: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('../pages/home/home-page.component').then(
        (m) => m.HomePageComponent,
      ),
  },
  {
    path: 'dashboards',
    loadChildren: () =>
      import('../pages/dashboards/dashboards.routes').then((m) => m.ROUTES),
  },
  {
    path: 'settings',
    loadChildren: () =>
      import('../pages/settings/settings.routes').then((m) => m.ROUTES),
  },
  {
    path: 'alerting',
    loadChildren: () =>
      import('../pages/alerting/alerting.routes').then((m) => m.ROUTES),
  },
  {
    path: '**',
    redirectTo: '',
  },
];
