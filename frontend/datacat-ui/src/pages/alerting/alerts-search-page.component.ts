import { Component } from '@angular/core';
import { PanelModule } from 'primeng/panel';
import { DividerModule } from 'primeng/divider';
import { FoundAlertsTableComponent } from '../../widgets/alerting/found-alerts-table.component';
import { AlertsFilteringComponent } from '../../widgets/alerting/alerts-filtering.component';

@Component({
  selector: 'alerts-search-page',
  standalone: true,
  imports: [
    PanelModule,
    DividerModule,
    FoundAlertsTableComponent,
    AlertsFilteringComponent,
  ],
  templateUrl: './alerts-search-page.component.html',
  styleUrl: './alerts-search-page.component.scss',
})
export class AlertsSearchPageComponent {}
