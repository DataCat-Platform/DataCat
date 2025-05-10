import { Component } from '@angular/core';
import { TableLazyLoadEvent, TableModule } from 'primeng/table';
import { Dashboard } from '../../../entities';
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
import * as urls from '../../../shared/common/urls';

@Component({
  standalone: true,
  selector: './datacat-dashboards-list',
  templateUrl: './dashboards-list.component.html',
  styleUrl: './dashboards-list.component.scss',
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
  ],
})
export class DashboardsListComponent {
  protected isDashboardViewDialogVisible = false;
  protected viewedDashboard?: Dashboard;

  protected addTagControl = new FormControl<string>('');

  protected filtersForm = new FormGroup({
    tags: new FormControl<string[]>([]),
  });

  protected get filtersFormTags(): string[] {
    return this.filtersForm.get('tags')?.value || [];
  }

  protected hasNextPage = false;
  protected hasPreviousPage = false;
  protected totalPagesCount = 0;
  protected totalDashboardsCount = 0;
  protected dashboardsPerPageCount = 5;
  protected currentPage = 1;
  protected dashboards: Dashboard[] = [];

  constructor(
    private router: Router,
    private apiService: ApiService,
    private loggerService: ToastLoggerService,
  ) {
    this.refreshDashboards();
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

  protected viewDashboard(dashboard: Dashboard) {
    this.isDashboardViewDialogVisible = true;
    this.viewedDashboard = dashboard;
  }

  protected editDashboard(dashboard: Dashboard) {
    this.router.navigateByUrl(urls.dashboardUrl(dashboard.id));
  }

  protected get searchFilters(): SearchFilters {
    const filters = {
      filters: [],
      sort: undefined,
    } as ISearchFilters;

    if (this.filtersFormTags.length !== 0) {
      filters.filters!.push({
        key: 'tags',
        value: this.filtersFormTags,
        matchMode: MatchMode.Contains,
        fieldType: SearchFieldType.Array,
      } as SearchFilter);
    }

    return filters as SearchFilters;
  }

  protected onLazyLoad(event: TableLazyLoadEvent) {
    if (event.first !== undefined && event.rows) {
      this.currentPage = Math.floor(event.first / event.rows) + 1;
      this.dashboardsPerPageCount = event.rows;
      this.refreshDashboards();
    }
  }

  protected refreshDashboards() {
    this.filtersForm.disable();
    this.apiService
      .postApiV1DashboardSearch(
        this.searchFilters,
        this.currentPage,
        this.dashboardsPerPageCount,
      )
      .pipe(
        finalize(() => {
          this.filtersForm.enable();
        }),
      )
      .subscribe({
        next: (data) => {
          this.totalDashboardsCount = data.totalCount || 0;
          this.totalPagesCount = data.totalPages || 0;
          this.hasNextPage = data.hasNextPage || false;
          this.hasPreviousPage = data.hasPreviousPage || false;
          this.currentPage = data.pageNumber || 0;
          this.dashboards =
            data.items?.map<Dashboard>((item) => {
              return {
                id: item.id || '',
                name: item.name || '',
                description: item.description || '',
                panels: [],
                createdAt: item.createdAt,
                lastUpdatedAt: item.updatedAt,
              };
            }) || [];
        },
        error: (e) => {
          this.loggerService.error(e);
        },
      });
  }
}
