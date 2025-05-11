import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {
    ApiService,
    MatchMode,
    SearchDataSourcesResponse,
    SearchFieldType,
    SearchFilter,
    SearchFilters
} from "../../../shared/services/datacat-generated-client";
import {catchError, debounceTime, distinctUntilChanged, Subject, switchMap, tap} from "rxjs";
import {DropdownModule} from "primeng/dropdown";
import {PrimeTemplate} from "primeng/api";
import {ProgressSpinner} from "primeng/progressspinner";
import {FormsModule} from "@angular/forms";
import {ToastLoggerService} from "../../../shared/services/toast-logger.service";

@Component({
    selector: 'app-traces-data-source-selector',
    standalone: true,
    imports: [
        DropdownModule,
        PrimeTemplate,
        ProgressSpinner,
        FormsModule
    ],
    templateUrl: './traces-data-source-selector.component.html',
    styleUrl: './traces-data-source-selector.component.scss'
})
export class TracesDataSourceSelectorComponent implements OnInit {
    @Input() selectedDataSource: string | undefined | null = null;
    @Output() selectedDataSourceChange = new EventEmitter<string>();

    dataSources: SearchDataSourcesResponse[] = [];
    searchTerm$ = new Subject<string>();
    loading = false;
    firstLoad = true;

    constructor(
        private apiService: ApiService,
        private toastLoggerService: ToastLoggerService
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

            }),
            catchError(err => {
                this.toastLoggerService.error(err.message);
                return err;
            }),
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
                value: 'Traces',
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
