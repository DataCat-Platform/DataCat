import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () => import("./features/home/pages/home-page/home-page.component").then(m => m.HomePageComponent),
  },
  {
    path: 'dashboards',
    loadChildren: () => import("./features/dashboards/dashboards.route")
  }
];
