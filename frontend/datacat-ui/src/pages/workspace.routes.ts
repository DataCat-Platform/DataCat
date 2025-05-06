import {Routes} from '@angular/router';
import {namespaceSelectedGuardFn} from '../shared/guards/namespace-selected.guard';
import * as urls from '../shared/common/urls';

export const ROUTES: Routes = [
    {
        path: urls.HOME_URL,
        loadComponent: () =>
            import('../widgets/workspace/home').then((m) => m.HomeComponent),
    },
    {
        path: urls.FORBIDDEN_URL,
        loadComponent: () =>
            import('../pages/forbidden').then((m) => m.ForbiddenComponent),
    },
    {
        path: '',
        canActivate: [namespaceSelectedGuardFn],
        children: [
            {
                path: urls.SETTINGS_URL,
                loadComponent: () =>
                    import('../processes/settings').then((m) => m.SettingsComponent),
            },
            {
                path: urls.ALERTS_EXPLORER_URL,
                loadComponent: () =>
                    import('../processes/explore-alerts').then(
                        (m) => m.AlertsExplorerComponent,
                    ),
            },
            {
                path: urls.ALERT_EDIT_URL,
                loadComponent: () =>
                    import('../processes/manage-alert').then(
                        (m) => m.ManageAlertComponent,
                    ),
            },
            {
                path: urls.ALERT_VIEW_URL,
                loadComponent: () =>
                    import('../processes/view-alert').then((m) => m.ViewAlertComponent),
            },
            {
                path: urls.NOTIFICATIONS_EXPLORER_URL,
                loadComponent: () =>
                    import('../processes/explore-notifications').then(
                        (m) => m.NotificationsExplorerComponent,
                    ),
            },
            {
                path: urls.NOTIFICATION_GROUP_EDIT_URL,
                loadComponent: () =>
                    import('../processes/manage-notification-group').then(
                        (m) => m.ManageNotificationGroupComponent,
                    ),
            },
            // {
            //   path: urls.NOTIFICATION_TEMPLATE_EDIT_URL,
            //   loadComponent: () => import('../processes/manage-notification-template').then(
            //     m => m.Mana
            //   )
            // }
        ],
    },
    {
        path: '**',
        redirectTo: urls.HOME_URL,
    },
];
