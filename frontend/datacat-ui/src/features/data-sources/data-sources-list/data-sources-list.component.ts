import {Component, Input, OnInit, SimpleChanges} from '@angular/core';
import {DataSourcesFilter} from "../../../entities/data-sources/data-sources-filter";
import {Router} from "@angular/router";
import {
    ApiService,
    ISearchDataSourcesResponse,
    ISearchFilters,
    MatchMode,
    SearchFieldType,
    SearchFilter,
    SearchFilters
} from "../../../shared/services/datacat-generated-client";
import {LazyLoadEvent} from "primeng/api";
import {TableModule} from "primeng/table";
import {Button} from "primeng/button";
import {Tooltip} from "primeng/tooltip";
import {ToastLoggerService} from "../../../shared/services/toast-logger.service";
import {Tag} from "primeng/tag";
import {capitalizeFirstLetter} from "../../../shared/utils/capitalizeFirstLetter";

@Component({
    selector: 'app-data-sources-list',
    standalone: true,
    imports: [
        TableModule,
        Button,
        Tooltip,
        Tag,
    ],
    templateUrl: './data-sources-list.component.html',
    styleUrl: './data-sources-list.component.scss'
})
export class DataSourcesListComponent implements OnInit {
    @Input() protected totalRecords = 0;
    @Input() protected rows = 10;
    @Input() protected dataSources: ISearchDataSourcesResponse[] = [];
    @Input() protected loading = false;
    protected currentPage = 1; // Текущая страница

    constructor(
        private router: Router,
        private apiService: ApiService,
        private toastLoggerService: ToastLoggerService,
    ) {
    }

    @Input() public set filter(filter: DataSourcesFilter | undefined) {
        if (filter) {
            this.currentPage = 1;
            this.refreshDataSources(filter, this.currentPage, this.rows);
        }
    }

    ngOnInit() {
        this.currentPage = 1;
        this.refreshDataSources(this.filter, this.currentPage, this.rows);
    }

    ngOnChanges(changes: SimpleChanges) {
        if (changes['filter'] && !changes['filter'].firstChange) {
            this.currentPage = 1;
        }
    }

    protected editDataSource(dataSourceId: string) {
        this.router.navigate(['/data-sources', dataSourceId, 'edit']);
    }

    protected loadData(event: LazyLoadEvent) {
        if (event.first !== undefined && event.rows !== undefined) {
            this.currentPage = Math.floor(event.first / event.rows) + 1;
            this.rows = event.rows;
            this.refreshDataSources(this.filter, this.currentPage, this.rows);
        }
    }

    protected getSeverity(purpose: string): 'info' | 'warn' | 'success' | 'secondary' {
        switch (purpose?.toLowerCase()) {
            case 'metrics':
                return 'info';
            case 'logs':
                return 'warn';
            case 'traces':
                return 'success';
            default:
                return 'secondary';
        }
    }

    private refreshDataSources(filter: DataSourcesFilter | undefined, page: number, pageSize: number) {
        this.loading = true;
        const request = this.convertToApiFilters(filter);
        console.log(request);

        this.apiService.postApiV1DataSourceSearch(request, page, pageSize).subscribe({
            next: (result) => {
                this.dataSources = result.items ?? [];
                this.totalRecords = result.totalCount ?? 0;
                this.loading = false;
            },
            error: (err) => {
                this.toastLoggerService.error(err.message);
                console.error('Failed to load data sources', err);
                this.loading = false;
            }
        });
    }

    private convertToApiFilters(filter: DataSourcesFilter | undefined): SearchFilters {
        let request = {
            filters: [],
            sort: undefined
        } as ISearchFilters;

        if (filter?.name) {
            request.filters!.push({
                key: 'name',
                value: filter.name,
                matchMode: MatchMode.Equals,
                fieldType: SearchFieldType.String
            } as SearchFilter);
        }

        if (filter?.typeId) {
            request.filters!.push({
                key: 'typeId',
                value: filter.typeId,
                matchMode: MatchMode.Equals,
                fieldType: SearchFieldType.Number
            } as SearchFilter);
        }

        if (filter?.purpose) {
            request.filters!.push({
                key: 'purpose',
                value: capitalizeFirstLetter(filter.purpose),
                matchMode: MatchMode.Equals,
                fieldType: SearchFieldType.String
            } as SearchFilter);
        }

        return request as SearchFilters;
    }
}
