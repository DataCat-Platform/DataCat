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

@Component({
    selector: 'app-explore-logs',
    standalone: true,
    imports: [
        Panel,
        LogsFilterComponent,
        LogsListComponent
    ],
    templateUrl: './explore-logs.component.html',
    styleUrl: './explore-logs.component.scss'
})
export class ExploreLogsComponent {
    currentFilter: ISearchLogsRequest = {
        dataSourceName: 'default',
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

    constructor(private apiService: ApiService) {
    }

    ngOnInit() {
        this.loadLogs();
    }

    onFilterChange(filter: ISearchLogsRequest) {
        this.currentFilter = {...filter, page: 1};
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

    private loadLogs() {
        this.apiService.postApiV1LogsSearch(this.currentFilter as SearchLogsRequest).subscribe({
            next: (page) => {
                this.logs = page.items!;
                this.pagination = {
                    page: page.pageNumber!,
                    pageSize: page.pageSize!,
                    totalCount: page.totalCount!,
                    totalPages: page.totalPages!
                };
            },
            error: (err) => console.error('Error loading logs:', err)
        });
    }
}
