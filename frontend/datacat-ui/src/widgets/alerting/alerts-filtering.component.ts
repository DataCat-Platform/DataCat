import { Component, OnInit } from '@angular/core';
import { TableModule } from 'primeng/table';
import { TooltipModule } from 'primeng/tooltip';
import { CommonModule } from '@angular/common';
import { SearchAlertsService } from '../../features/alerting/search-alerts/search-alerts.service';
import { FilterComponent } from '../../shared/ui';

@Component({
  selector: 'alerts-filtering',
  standalone: true,
  imports: [FilterComponent],
  templateUrl: './alerts-filtering.component.html',
})
export class AlertsFilteringComponent implements OnInit {
  availableFilters!: any[];

  constructor(protected searchAlertsService: SearchAlertsService) {}

  ngOnInit() {
    this.searchAlertsService.searchAlerts();
  }
}
