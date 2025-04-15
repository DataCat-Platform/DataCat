import { Routes } from '@angular/router';
import { AlertsComponent } from './alerts-explore-page.component';
import { ChannelsComponent } from './channels-explore-page.component';

export const ROUTES: Routes = [
  {
    path: 'alerts',
    component: AlertsComponent,
  },
  {
    path: 'channels',
    component: ChannelsComponent,
  },
];
