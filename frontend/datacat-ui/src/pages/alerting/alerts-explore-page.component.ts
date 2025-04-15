import { Component, OnInit } from '@angular/core';
import { PanelModule } from 'primeng/panel';
import { DividerModule } from 'primeng/divider';
import { TableModule } from 'primeng/table';
import { TooltipModule } from 'primeng/tooltip';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'alerts',
  standalone: true,
  imports: [
    PanelModule,
    DividerModule,
    TableModule,
    TooltipModule,
    CommonModule,
  ],
  templateUrl: './alerts-explore-page.component.html',
})
export class AlertsComponent implements OnInit {
  alerts: any;
  availableFilters!: any[];

  ngOnInit() {
    this.alerts = [
      {
        id: 1,
        brief: 'Response time is too large.',
        createdAt: Date.now(),
        lastStatus: 'OK',
        lastTimeExecuted: Date.now(),
        nextExecutionTime: Date.now(),
      },
    ];
    this.availableFilters = [];
  }
}
