import { Component } from '@angular/core';
import { TableModule } from 'primeng/table';
import { Dashboard } from '../../../entities';
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
  ],
})
export class DashboardsListComponent {
  protected addTagControl = new FormControl<string>('');

  protected get tagToAdd(): string {
    return this.addTagControl.value || '';
  }

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
  protected currentPage = 0;
  protected dashboards: Dashboard[] = [];

  constructor(
    private router: Router,
    private apiService: ApiService,
    private loggerService: ToastLoggerService,
  ) {
    this.refreshDashboards();
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

  protected viewDashboard(dashboardId: string) {
    this.router.navigateByUrl(urls.dashboardUrl(dashboardId));
  }

  protected get searchFilters(): SearchFilters {
    const filters: ISearchFilter[] = [];

    return {
      filters: filters as any,
    } as any;
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
          console.log(data);
          this.totalDashboardsCount = data.totalCount || 0;
          this.totalPagesCount = data.totalPages || 0;
          this.hasNextPage = data.hasNextPage || false;
          this.hasPreviousPage = data.hasPreviousPage || false;
          this.currentPage = data.pageNumber || 0;
          this.dashboards = [];
        },
        error: (e) => {
          console.log(e);
          this.loggerService.error(e);
        },
      });
  }

  protected loadPage(event: any) {}
}
