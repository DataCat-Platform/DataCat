import {Component, OnInit} from '@angular/core';
import {DynamicDialogConfig} from "primeng/dynamicdialog";
import {
    ApiService,
    ISearchLogsRequest,
    SearchLogsRequest,
    SpanEntry
} from "../../../shared/services/datacat-generated-client";
import {Button} from "primeng/button";
import {PrimeTemplate} from "primeng/api";
import {TableModule} from "primeng/table";
import {sanitizeMessage} from "../../../shared/utils/sanitizeMessage";
import {LogsDetailsComponent} from "../../logs/logs-details/logs-details.component";
import {AppDialogService} from "../../../shared/services/app-dialog.service";
import {
    LogsDataSourceSelectorComponent
} from "../../logs/logs-data-source-selector/logs-data-source-selector.component";

@Component({
    selector: 'app-span-details-dialog',
    standalone: true,
    imports: [
        Button,
        PrimeTemplate,
        TableModule,
        LogsDataSourceSelectorComponent
    ],
    templateUrl: './span-details-dialog.component.html',
    styleUrl: './span-details-dialog.component.scss'
})
export class SpanDetailsDialogComponent implements OnInit {
    span: SpanEntry;
    logs: any[] = [];
    pagination = {page: 1, pageSize: 10, totalCount: 0};
    flattenedData: Array<{ key: string; value: any }> = [];
    dataSourceName = '';

    protected readonly sanitizeMessage = sanitizeMessage;

    constructor(
        public config: DynamicDialogConfig,
        private apiService: ApiService,
        private appDialogService: AppDialogService,
    ) {
        this.span = config.data;
        this.flattenObject(this.span, '');
    }

    get objectKeys() {
        return Object.keys;
    }

    ngOnInit(): void {
        this.loadLogs();
    }

    loadLogs() {
        if (!this.dataSourceName) return;

        const request: ISearchLogsRequest = {
            dataSourceName: this.dataSourceName,
            pageSize: 100,
            page: 1,
            sortAscending: false,
            customFilters: {
                'fields.SpanId.keyword': this.span.spanId!
            }
        };

        this.apiService.postApiV1LogsSearch(request as SearchLogsRequest)
            .subscribe(response => {
                this.logs = response.items!;
                this.pagination.totalCount = response.totalCount!;
            });
    }

    showLogsDetails(additionalFields: any) {
        this.appDialogService.showDialog(LogsDetailsComponent,
            'Additional Fields',
            additionalFields || {}
        )
    }

    onDataSourceChange(dataSourceName: string) {
        this.dataSourceName = dataSourceName;
        this.loadLogs();
    }

    private flattenObject(obj: any, prefix: string) {
        for (const key of Object.keys(obj)) {
            const fullKey = prefix ? `${prefix}.${key}` : key;
            if (typeof obj[key] === 'object' && obj[key] !== null && !Array.isArray(obj[key])) {
                this.flattenObject(obj[key], fullKey);
            } else if (Array.isArray(obj[key])) {
                this.flattenArray(obj[key], fullKey);
            } else {
                this.flattenedData.push({
                    key: fullKey,
                    value: this.formatValue(key, obj[key])
                });
            }
        }
    }

    private flattenArray(arr: any[], prefix: string) {
        arr.forEach((item, index) => {
            const fullKey = `${prefix}[${index}]`;
            if (typeof item === 'object' && item !== null) {
                this.flattenObject(item, fullKey);
            } else {
                this.flattenedData.push({
                    key: fullKey,
                    value: this.formatValue(fullKey, item)
                });
            }
        });
    }

    private formatValue(key: string, value: any): any {
        if (key.toLowerCase().includes('time') || key.toLowerCase().includes('timestamp')) {
            return new Date(value).toLocaleString();
        }
        return value;
    }
}
