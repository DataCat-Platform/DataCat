import { Component } from '@angular/core';
import { TableModule } from 'primeng/table';
import { Alert, AlertStatus, DataSource } from '../../../entities';
import { TagModule } from 'primeng/tag';
import { CommonModule } from '@angular/common';
import { ButtonModule } from 'primeng/button';
import { Router } from '@angular/router';
import * as urls from '../../../shared/common/urls';
import { finalize } from 'rxjs';
import {
  ApiService,
  ISearchFilter,
  SearchFilters,
} from '../../../shared/services/datacat-generated-client';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { SelectButtonModule } from 'primeng/selectbutton';
import { SelectModule } from 'primeng/select';
import { ChipModule } from 'primeng/chip';
import { ToastLoggerService } from '../../../shared/services/toast-logger.service';
import { InputGroupModule } from 'primeng/inputgroup';
import { InputTextModule } from 'primeng/inputtext';
import { InputGroupAddonModule } from 'primeng/inputgroupaddon';

@Component({
  standalone: true,
  selector: './datacat-alerts-list',
  templateUrl: './alerts-list.component.html',
  styleUrl: './alerts-list.component.scss',
  imports: [
    TableModule,
    TagModule,
    CommonModule,
    ButtonModule,
    ReactiveFormsModule,
    SelectButtonModule,
    SelectModule,
    ChipModule,
    InputGroupModule,
    InputGroupAddonModule,
    InputTextModule,
  ],
})
export class AlertsListComponent {
  protected addTagControl = new FormControl<string>('');

  protected get tagToAdd(): string {
    return this.addTagControl.value || '';
  }

  protected filtersForm = new FormGroup({
    status: new FormControl<AlertStatus | null>(null),
    dataSourceId: new FormControl<string | null>(null),
    tags: new FormControl<string[]>([]),
  });

  protected get filtersFormStatus(): AlertStatus | null {
    return this.filtersForm.get('status')?.value || null;
  }

  protected get dataSourceId(): string | null {
    return this.filtersForm.get('dataSourceId')?.value || null;
  }

  protected get filtersFormTags(): string[] {
    return this.filtersForm.get('tags')?.value || [];
  }

  protected possibleAlertStatuses = Object.values(AlertStatus);
  protected possibleDataSources: DataSource[] = [];

  protected hasNextPage = false;
  protected hasPreviousPage = false;
  protected totalPagesCount = 0;
  protected totalAlertsCount = 0;
  protected alertsPerPageCount = 5;
  protected currentPage = 0;
  protected alerts: Alert[] = [];

  constructor(
    private router: Router,
    private apiService: ApiService,
    private loggerService: ToastLoggerService,
  ) {
    this.refreshAlerts();
    this.refreshPossibleDataSources();
  }

  protected addFilterTag(tag: string) {
    const previousTags = this.filtersForm.get('tags')?.value || [];
    if (tag && !previousTags.includes(tag)) {
      this.filtersForm.get('tags')?.setValue([...previousTags, tag]);
    }
  }

  protected removeFilterTag(tag: string) {
    const previousTags = this.filtersForm.get('tags')?.value || [];
    this.filtersForm
      .get('tags')
      ?.setValue(previousTags.filter((t) => t !== tag));
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

  protected get searchFilters(): SearchFilters {
    const filters: ISearchFilter[] = [];

    return {
      filters: filters as any,
    } as any;
  }

  protected refreshAlerts() {
    this.filtersForm.disable();
    this.apiService
      .postApiV1AlertSearch(
        this.searchFilters,
        this.currentPage,
        this.alertsPerPageCount,
      )
      .pipe(
        finalize(() => {
          this.filtersForm.enable();
        }),
      )
      .subscribe({
        next: (data) => {
          this.totalAlertsCount = data.totalCount || 0;
          this.totalPagesCount = data.totalPages || 0;
          this.hasNextPage = data.hasNextPage || false;
          this.hasPreviousPage = data.hasPreviousPage || false;
          this.currentPage = data.pageNumber || 0;
          this.alerts = [];
        },
        error: (e) => {
          this.loggerService.error(e);
        },
      });
  }

  protected refreshPossibleDataSources() {
    // TODO
  }
}
