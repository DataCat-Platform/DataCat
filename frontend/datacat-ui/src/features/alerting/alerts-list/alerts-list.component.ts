import {Component, Input} from '@angular/core';
import {TableModule} from 'primeng/table';
import {Alert, AlertStatus} from '../../../entities';
import {TagModule} from 'primeng/tag';
import {CommonModule} from '@angular/common';
import {ButtonModule} from 'primeng/button';
import {Router} from '@angular/router';
import * as urls from '../../../shared/common/urls';
import {AlertsFilter} from '../../../entities/alerting/alerts-filter.types';
import {from} from 'rxjs';
import {FAKE_ALERT} from '../../../shared/mock/fakes';
import {ApiService} from "../../../shared/services/datacat-generated-client";

@Component({
    standalone: true,
    selector: './datacat-alerts-list',
    templateUrl: './alerts-list.component.html',
    styleUrl: './alerts-list.component.scss',
    imports: [TableModule, TagModule, CommonModule, ButtonModule],
})
export class AlertsListComponent {
    @Input() protected totalAlertsCount = 0;
    @Input() protected alertsPerPageCount = 5;
    @Input() protected alerts: Alert[] = [];

    constructor(
        private router: Router,
        private apiService: ApiService,
    ) {
    }

    @Input() public set filter(filter: AlertsFilter | undefined) {
        if (filter) {
            this.refreshAlerts(filter);
        }
    }

    protected viewAlert(alertId: string) {
        this.router.navigateByUrl(urls.alertViewUrl(alertId));
    }

    protected getSeverityForStatus(
        status: AlertStatus,
    ): 'success' | 'danger' | 'secondary' | 'info' {
        {
            switch (status) {
                case AlertStatus.OK: {
                    return 'success';
                }
                case AlertStatus.FIRING:
                case AlertStatus.ERROR: {
                    return 'danger';
                }
                case AlertStatus.PENDING: {
                    return 'info';
                }
                case AlertStatus.MUTED: {
                    return 'secondary';
                }
                default: {
                    return 'secondary';
                }
            }
        }
    }

    private refreshAlerts(filter: AlertsFilter) {
        // TODO: call API
        from([[FAKE_ALERT]]).subscribe({
            next: (alerts) => (this.alerts = alerts),
            error: () => {
                // TODO
            },
        });
    }
}
