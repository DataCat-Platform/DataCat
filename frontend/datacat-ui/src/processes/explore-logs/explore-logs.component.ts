import {Component} from '@angular/core';
import {Panel} from "primeng/panel";
import {
    ApiService,
    ISearchLogsRequest,
    LogEntry,
    SearchLogsRequest
} from "../../shared/services/datacat-generated-client";
import {LogsFilterComponent} from "../../features/logs/logs-filter/logs-filter.component";
import {LogsListComponent} from "../../features/logs/logs-list/logs-list.component";
import {DropdownModule} from "primeng/dropdown";
import {FormsModule} from "@angular/forms";
import {ToastLoggerService} from "../../shared/services/toast-logger.service";
import {
    LogsDataSourceSelectorComponent
} from "../../features/logs/logs-data-source-selector/logs-data-source-selector.component";
import {catchError, finalize, tap} from "rxjs";
import {ProgressSpinner} from "primeng/progressspinner";
import {formatCustomProblemDetails} from "../../shared/utils/formatCustomProblemDetails";

@Component({
    selector: 'app-explore-logs',
    standalone: true,
    imports: [
        Panel,
        LogsFilterComponent,
        LogsListComponent,
        DropdownModule,
        FormsModule,
        LogsDataSourceSelectorComponent,
        ProgressSpinner
    ],
    templateUrl: './explore-logs.component.html',
    styleUrl: './explore-logs.component.scss'
})
export class ExploreLogsComponent {
    isLoading = false;

    currentFilter: ISearchLogsRequest = {
        dataSourceName: '',
        pageSize: 100,
        page: 1,
        sortAscending: false
    };

    logs: LogEntry[] = [];
    pagination = {
        page: 1,
        pageSize: 100,
        totalCount: 0,
        totalPages: 0
    };

    constructor(
        private apiService: ApiService,
        private toastLoggerService: ToastLoggerService) {
    }

    ngOnInit() {
        this.loadLogs();
    }

    onFilterChange(filter: ISearchLogsRequest) {
        const currentDataSource = this.currentFilter.dataSourceName;
        this.currentFilter = {
            ...filter,
            dataSourceName: currentDataSource,
            page: 1
        };
        this.loadLogs();
    }

    onPageChange(page: number) {
        this.currentFilter.page = page;
        this.loadLogs();
    }

    onSortChange(sort: { field: string; ascending: boolean }) {
        this.currentFilter.sortField = sort.field;
        this.currentFilter.sortAscending = sort.ascending;
        this.loadLogs();
    }

    onDataSourceChange(dataSourceName: string) {
        this.currentFilter.dataSourceName = dataSourceName;
        this.currentFilter.page = 1;
        this.loadLogs();
    }

    private loadLogs() {
        if (!this.currentFilter.dataSourceName) {
            return;
        }

        this.isLoading = true;

        this.apiService.postApiV1LogsSearch(this.currentFilter as SearchLogsRequest).pipe(
            finalize(() => this.isLoading = false),
            tap(page => {
                this.logs = page.items!;
                this.pagination = {
                    page: page.pageNumber!,
                    pageSize: page.pageSize!,
                    totalCount: page.totalCount!,
                    totalPages: page.totalPages!
                };
            }),
            catchError(error => {
                this.toastLoggerService.error(formatCustomProblemDetails(error));
                return error;
            })
        ).subscribe()
    }
}
