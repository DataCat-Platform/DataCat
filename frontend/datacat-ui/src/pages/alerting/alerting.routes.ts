import { Routes } from '@angular/router';
import { AlertsSearchPageComponent } from './alerts-search-page.component';
import { ChannelsComponent } from './channels-explore-page.component';

export const ROUTES: Routes = [
  {
    path: 'alerts',
    component: AlertsSearchPageComponent,
  },
  {
    path: 'channels',
    component: ChannelsComponent,
  },
];
