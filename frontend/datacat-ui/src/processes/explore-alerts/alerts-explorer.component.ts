import { Component, OnInit } from '@angular/core';
import { AlertsFilteringFormComponent } from '../../features/alerting/alerts-filtering';
import { AlertsCountsByStatusComponent } from '../../features/alerting/alerts-counts-by-status/alerts-counts-by-status.component';
import { AlertsListComponent } from '../../features/alerting/alerts-list/alerts-list.component';
import { PanelModule } from 'primeng/panel';
import { AlertsFilter } from '../../entities/alerting/alerts-filter.types';
import { CreateAlertButtonComponent } from '../../features/alerting/create-alert';

@Component({
  standalone: true,
  selector: 'datacat-alerts-explore',
  templateUrl: './alerts-explorer.component.html',
  styleUrl: './alerts-explorer.component.scss',
  imports: [
    AlertsFilteringFormComponent,
    AlertsCountsByStatusComponent,
    AlertsListComponent,
    PanelModule,
    CreateAlertButtonComponent,
  ],
})
export class AlertsExplorerComponent implements OnInit {
  protected filter?: AlertsFilter;

  ngOnInit() {}

  protected applyFilter(filter: AlertsFilter) {
    this.filter = filter;
  }
}
