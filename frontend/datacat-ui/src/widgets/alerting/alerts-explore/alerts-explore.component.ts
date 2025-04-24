import { Component, OnInit } from '@angular/core';
import { Panel } from 'primeng/panel';
import { Divider } from 'primeng/divider';
import { SelectButton } from 'primeng/selectbutton';
import { InputTextModule } from 'primeng/inputtext';
import { FormsModule } from '@angular/forms';
import { Select } from 'primeng/select';
import { AlertStatus } from '../../../entities/alerting.entities';
import { InputGroupModule } from 'primeng/inputgroup';
import { InputGroupAddonModule } from 'primeng/inputgroupaddon';
import { Tag } from 'primeng/tag';
import { TableModule } from 'primeng/table';
import { AlertsExploreService } from '../../../features/alerting/alerts-explore.service';
import { CommonModule } from '@angular/common';
import { Button } from 'primeng/button';
import { Router, RouterModule } from '@angular/router';

@Component({
  standalone: true,
  selector: 'datacat-alerts-explore',
  templateUrl: './alerts-explore.component.html',
  styleUrl: './alerts-explore.component.scss',
  imports: [
    Panel,
    Divider,
    SelectButton,
    InputTextModule,
    FormsModule,
    Select,
    InputGroupModule,
    InputGroupAddonModule,
    Tag,
    TableModule,
    CommonModule,
    Button,
    RouterModule,
  ],
})
export class AlertsSearchPageComponent implements OnInit {
  protected alertStatuses: string[] = Object.values(AlertStatus).map((s) =>
    s.toUpperCase(),
  );

  constructor(
    protected alertsExploreService: AlertsExploreService,
    private _router: Router,
  ) {}

  public ngOnInit() {
    this.alertsExploreService.searchAlerts();
  }

  public editAlert(id: string) {
    this._router.navigate(['alerting', 'alerts', id]);
  }

  protected severityFromStatus(
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
