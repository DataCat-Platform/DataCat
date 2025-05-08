import {Component} from '@angular/core';
import {Panel} from "primeng/panel";
import {
    TracesDataSourceSelectorComponent
} from "../../features/traces/traces-data-source-selector/traces-data-source-selector.component";
import {
    ApiService,
    ISearchTracesRequest,
    SearchTracesRequest,
    TraceEntry
} from "../../shared/services/datacat-generated-client";
import {catchError, finalize, tap} from "rxjs";
import {ToastLoggerService} from "../../shared/services/toast-logger.service";
import {ProgressSpinner} from "primeng/progressspinner";
import {TracesListComponent} from "../../features/traces/traces-list/traces-list.component";
import {TracesFilterComponent} from "../../features/traces/traces-filter/traces-filter.component";

@Component({
    selector: 'app-explore-traces',
    standalone: true,
    imports: [
        Panel,
        TracesDataSourceSelectorComponent,
        ProgressSpinner,
        TracesListComponent,
        TracesFilterComponent
    ],
    templateUrl: './explore-traces.component.html',
    styleUrl: './explore-traces.component.scss'
})
export class ExploreTracesComponent {
    isLoading = false;

    traces: TraceEntry[] = [];
    currentFilter: ISearchTracesRequest = {
        dataSourceName: '',
        service: '',
        operation: '',
        tags: undefined,
        start: new Date(Date.now() - 3600 * 1000),
        end: new Date(),
        limit: 20,
        minDuration: undefined,
        maxDuration: undefined
    } as ISearchTracesRequest;

    constructor(
        private apiService: ApiService,
        private toastLoggerService: ToastLoggerService,
    ) {
    }

    onDataSourceChange(dataSourceName: string) {
        this.currentFilter = {
            ...this.currentFilter,
            dataSourceName: dataSourceName
        };
    }

    onFilterChange(filter: any) {
        this.currentFilter = filter;
        this.loadTraces();
    }

    private loadTraces() {
        if (!this.currentFilter.dataSourceName) {
            return;
        }

        this.isLoading = true;

        this.apiService.postApiV1TracesSearch(this.currentFilter as SearchTracesRequest).pipe(
            finalize(() => this.isLoading = false),
            tap(traces => {
                this.traces = traces!;
            }),
            catchError(error => {
                this.toastLoggerService.error(error.message);
                return error;
            })
        ).subscribe()
    }
}
