import {Component, Input} from '@angular/core';
import {PanelModule} from 'primeng/panel';
import {DividerModule} from 'primeng/divider';
import {DeleteNotificationGroupButtonComponent} from '../../features/alerting/delete-notification-group';
import {EditNotificationGroupFormComponent} from '../../features/alerting/edit-notification-group';

@Component({
    standalone: true,
    selector: 'datacat-manage-notification-group',
    templateUrl: './manage-notification-group.component.html',
    styleUrl: './manage-notification-group.component.scss',
    imports: [
        PanelModule,
        EditNotificationGroupFormComponent,
        DeleteNotificationGroupButtonComponent,
        DividerModule,
    ],
})
export class ManageNotificationGroupComponent {
    @Input() notificationGroupId: string = '';
}
