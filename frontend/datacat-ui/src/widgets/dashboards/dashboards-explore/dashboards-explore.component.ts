import { Component } from '@angular/core';
import { Panel } from 'primeng/panel';
import { Divider } from 'primeng/divider';
import { Button } from 'primeng/button';
import { TableModule } from 'primeng/table';
import { CommonModule } from '@angular/common';
import { DashboardsExploreService } from '../../../features/dashboards/dashboards-explore.service';
import { Router } from '@angular/router';
import { Checkbox } from 'primeng/checkbox';
import { InputGroup } from 'primeng/inputgroup';
import { InputGroupAddon } from 'primeng/inputgroupaddon';
import { FormsModule } from '@angular/forms';
import { InputText } from 'primeng/inputtext';

@Component({
  standalone: true,
  selector: 'datacat-dashboards-explore',
  imports: [
    Panel,
    Divider,
    Button,
    TableModule,
    CommonModule,
    Checkbox,
    InputGroup,
    InputGroupAddon,
    FormsModule,
    InputText,
  ],
  templateUrl: './dashboards-explore.component.html',
  styleUrl: './dashboards-explore.component.scss',
})
export class DashboardsExploreComponent {
  constructor(
    protected dashboardsExploreService: DashboardsExploreService,
    protected router: Router,
  ) {
    this.dashboardsExploreService.refreshDashboards();
  }

  protected viewDashboard(dashboardId: string) {
    this.router.navigate(['dashboards', dashboardId]);
  }

  protected toggleStarred(dashboardId: string) {}
}
