import {Component, EventEmitter, Input, Output} from '@angular/core';
import {debounceTime, distinctUntilChanged, Subject, switchMap, tap} from "rxjs";
import {
    ApiService,
    MatchMode,
    SearchDataSourcesResponse,
    SearchFieldType,
    SearchFilter,
    SearchFilters
} from "../../../shared/services/datacat-generated-client";
import {DropdownModule} from "primeng/dropdown";
import {FormsModule} from "@angular/forms";
import {ProgressSpinner} from "primeng/progressspinner";

@Component({
    selector: 'app-logs-data-source-selector',
    standalone: true,
    imports: [
        DropdownModule,
        FormsModule,
        ProgressSpinner
    ],
    templateUrl: './logs-data-source-selector.component.html',
    styleUrl: './logs-data-source-selector.component.scss'
})
export class LogsDataSourceSelectorComponent {
    @Input() selectedDataSource: string | undefined | null = null;
    @Output() selectedDataSourceChange = new EventEmitter<string>();

    dataSources: SearchDataSourcesResponse[] = [];
    searchTerm$ = new Subject<string>();
    loading = false;
    firstLoad = true;

    constructor(
        private apiService: ApiService
    ) {
    }

    ngOnInit() {
        this.searchTerm$.pipe(
            debounceTime(300),
            distinctUntilChanged(),
            switchMap(term => this.searchDataSources(term))
        ).subscribe(data => {
            this.dataSources = data.items!;
            this.loading = false;
        });

        this.searchDataSources('').pipe(
            tap(data => {
                this.firstLoad = false;

                if (!data.items) {
                    return;
                }

                this.dataSources = data.items;
                if (this.firstLoad && data.items.length > 0 && !this.selectedDataSource) {
                    this.selectedDataSource = data.items[0].name!;
                    this.selectedDataSourceChange.emit(this.selectedDataSource);
                }

            })
        ).subscribe();
    }

    onSearch(event: { filter: string }) {
        this.loading = true;
        this.searchTerm$.next(event.filter);
    }

    onDataSourceChange() {
        this.selectedDataSourceChange.emit(this.selectedDataSource ?? '');
    }

    private searchDataSources(searchTerm: string) {
        const filters: SearchFilter[] = [
            ({
                key: 'purpose',
                value: 'Logs',
                matchMode: MatchMode.Equals,
                fieldType: SearchFieldType.String
            } as SearchFilter)
        ];

        if (searchTerm?.trim()) {
            filters.unshift({
                key: 'name',
                value: searchTerm,
                matchMode: MatchMode.Contains,
                fieldType: SearchFieldType.String
            } as SearchFilter);
        }

        const request = {
            filters: filters,
        } as SearchFilters

        return this.apiService.postApiV1DataSourceSearch(request, 1, 10);
    }
}
