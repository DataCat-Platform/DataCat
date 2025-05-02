import { Component, OnInit } from '@angular/core';
import { Panel } from 'primeng/panel';
import { Divider } from 'primeng/divider';
import { SelectButton } from 'primeng/selectbutton';
import { InputTextModule } from 'primeng/inputtext';
import { FormsModule } from '@angular/forms';
import { Select } from 'primeng/select';
import { AlertStatus } from '../../../entities/alerts.entities';
import { InputGroupModule } from 'primeng/inputgroup';
import { InputGroupAddonModule } from 'primeng/inputgroupaddon';
import { Tag } from 'primeng/tag';
import { TableModule, TablePageEvent } from 'primeng/table';
import { AlertsExplorerService } from '../../../features/alerting/alerts-explorer';
import { CommonModule } from '@angular/common';
import { Button } from 'primeng/button';
import { Router, RouterModule } from '@angular/router';
import { ChipModule } from 'primeng/chip';
import { PopoverModule } from 'primeng/popover';
import * as urls from '../../../entities/urls';

@Component({
  standalone: true,
  selector: 'datacat-alerts-explore',
  templateUrl: './alerts-explorer.component.html',
  styleUrl: './alerts-explorer.component.scss',
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
    ChipModule,
    PopoverModule,
  ],
})
export class AlertsExplorerComponent implements OnInit {
  protected alertStatuses: string[] = Object.values(AlertStatus).map((s) =>
    s.toUpperCase(),
  );

  protected tag: string = '';

  constructor(
    protected alertsExplorerService: AlertsExplorerService,
    private router: Router,
  ) {}

  public ngOnInit() {
    this.alertsExplorerService.refreshAlerts();
  }

  public pageChange(event: TablePageEvent) {
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
