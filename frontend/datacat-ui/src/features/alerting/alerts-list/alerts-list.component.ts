import { Component } from '@angular/core';
import { TableLazyLoadEvent, TableModule } from 'primeng/table';
import { Alert, AlertStatus, DataSource } from '../../../entities';
import { TagModule } from 'primeng/tag';
import { CommonModule } from '@angular/common';
import { ButtonModule } from 'primeng/button';
import { Router } from '@angular/router';
import { finalize } from 'rxjs';
import {
  ApiService,
  ISearchFilters,
  MatchMode,
  SearchFieldType,
  SearchFilter,
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
import { TooltipModule } from 'primeng/tooltip';
import { DialogModule } from 'primeng/dialog';
import { DataSourceSelectComponent } from '../../../shared/ui/data-source-select/data-source-select.component';
import * as urls from '../../../shared/common/urls';
import { LazyLoadEvent } from 'primeng/api';

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
    TooltipModule,
    DialogModule,
    DataSourceSelectComponent,
  ],
})
export class AlertsListComponent {
  protected isAlertViewDialogVisible = false;
  protected viewedAlert?: Alert;

  protected addTagControl = new FormControl<string>('');

  protected filtersForm = new FormGroup({
    status: new FormControl<AlertStatus | null>(null),
    dataSourceId: new FormControl<string | null>(null),
    tags: new FormControl<string[]>([]),
  });

  protected get filtersFormStatus(): AlertStatus | null {
    return this.filtersForm.get('status')?.value || null;
  }

  protected get filtersFormDataSourceId(): string | null {
    return this.filtersForm.get('dataSourceId')?.value || null;
  }

  protected get filtersFormTags(): string[] {
    return this.filtersForm.get('tags')?.value || [];
  }

  protected possibleAlertStatuses = Object.values(AlertStatus);

  protected hasNextPage = false;
  protected hasPreviousPage = false;
  protected totalPagesCount = 0;
  protected totalAlertsCount = 0;
  protected alertsPerPageCount = 5;
  protected currentPage = 1;
  protected alerts: Alert[] = [];

  constructor(
    private router: Router,
    private apiService: ApiService,
    private loggerService: ToastLoggerService,
  ) {
    this.refreshAlerts();
  }

  protected addFilterTag() {
    const tag = this.addTagControl.value;
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

  protected viewAlert(alert: Alert) {
    this.isAlertViewDialogVisible = true;
    this.viewedAlert = alert;
  }

  protected editAlert(alert: Alert) {
    this.router.navigateByUrl(urls.alertEditUrl(alert.id));
  }

  protected getSeverityForStatus(
    status?: AlertStatus,
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
    const filters = {
      filters: [],
      sort: undefined,
    } as ISearchFilters;

    if (this.filtersFormStatus) {
      filters.filters!.push({
        key: 'status',
        value: this.filtersFormStatus,
        matchMode: MatchMode.Equals,
        fieldType: SearchFieldType.String,
      } as SearchFilter);
    }

    if (this.filtersFormTags.length !== 0) {
      filters.filters!.push({
        key: 'tags',
        value: this.filtersFormTags,
        matchMode: MatchMode.Contains,
        fieldType: SearchFieldType.Array,
      } as SearchFilter);
    }

    if (this.filtersFormDataSourceId) {
      filters.filters!.push({
        key: 'dataSourceId',
        value: this.filtersFormDataSourceId,
        matchMode: MatchMode.Equals,
        fieldType: SearchFieldType.String,
      } as SearchFilter);
    }

    return filters as SearchFilters;
  }

  protected onLazyLoad(event: TableLazyLoadEvent) {
    if (event.first !== undefined && event.rows) {
      this.currentPage = Math.floor(event.first / event.rows) + 1;
      this.alertsPerPageCount = event.rows;
      this.refreshAlerts();
    }
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
          this.alerts =
            data.items?.map<Alert>((item) => {
              return {
                id: item.id || '',
                template: item.template || '',
                description: item.description || '',
                query: item.rawQuery || '',
                status: (item.status as AlertStatus) || AlertStatus.OK,
                dataSourceId: item.dataSource?.id || '',
                notificationGroupId: item.notificationChannelGroup?.id || '',
                prevExecutionTime:
                  item.previousExecutionTime?.getUTCMilliseconds() || 0,
                nextExecutionTime:
                  item.nextExecutionTime?.getUTCMilliseconds() || 0,
                notificationTriggerPeriod: item.waitTimeBeforeAlerting || '',
                executionInterval: item.repeatInterval || '',
              };
            }) || [];
        },
        error: (e) => {
          this.loggerService.error(e);
        },
      });
  }
}
