import {Component, OnInit} from '@angular/core';
import {AlertsCountsByStatus} from './alerts-counts-by-status.types';
import {TagModule} from 'primeng/tag';
import {AlertStatus} from '../../../entities';
import {TooltipModule} from 'primeng/tooltip';
import {from} from 'rxjs';
import {FAKE_ALERTS_COUNTS_BY_STATUS} from '../../../shared/mock/fakes';

@Component({
    standalone: true,
    selector: 'datacat-alerts-counts-by-status',
    templateUrl: './alerts-counts-by-status.component.html',
    styleUrl: './alerts-counts-by-status.component.scss',
    imports: [TagModule, TooltipModule],
})
export class AlertsCountsByStatusComponent implements OnInit {
    protected alertsCountsByStatus?: AlertsCountsByStatus;

    ngOnInit() {
        this.loadAlertsCountsByStatus();
    }

    protected loadAlertsCountsByStatus() {
        // TODO: add API call
        from([FAKE_ALERTS_COUNTS_BY_STATUS]).subscribe({
            next: (alertsCountsByStatus) => {
                this.alertsCountsByStatus = alertsCountsByStatus;
            },
            error: () => {
                // TODO
            },
        });
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
}
