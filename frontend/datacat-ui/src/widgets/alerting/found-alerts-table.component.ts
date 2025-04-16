import { Component } from '@angular/core';
import { TableModule } from 'primeng/table';
import { TooltipModule } from 'primeng/tooltip';
import { CommonModule } from '@angular/common';
import { TagModule } from 'primeng/tag';
import { SearchAlertsService } from '../../features/alerting/search-alerts/search-alerts.service';
import { AlertStatus } from '../../entities/alerting/alerting.types';

@Component({
  selector: 'found-alerts-table',
  standalone: true,
  imports: [TableModule, TooltipModule, CommonModule, TagModule],
  templateUrl: './found-alerts-table.component.html',
})
export class FoundAlertsTableComponent {
  availableFilters!: any[];

  constructor(protected searchAlertsService: SearchAlertsService) {}

  protected severityFromStatus(
    status: AlertStatus,
  ): 'success' | 'danger' | 'secondary' {
    switch (status) {
      case AlertStatus.OK: {
        return 'success';
      }
      case AlertStatus.FIRE: {
        return 'danger';
      }
      case AlertStatus.ERROR: {
        return 'secondary';
      }
    }
  }
}
