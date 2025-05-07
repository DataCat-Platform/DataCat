import {Component, EventEmitter, Input, Output} from '@angular/core';
import {LogEntry} from "../../../shared/services/datacat-generated-client";
import {TableModule} from "primeng/table";
import {DatePipe, NgForOf, NgIf} from "@angular/common";
import {Tag} from "primeng/tag";
import {ButtonDirective} from "primeng/button";

@Component({
    selector: 'app-logs-list',
    standalone: true,
    imports: [
        TableModule,
        DatePipe,
        Tag,
        NgForOf,
        ButtonDirective,
        NgIf
    ],
    templateUrl: './logs-list.component.html',
    styleUrl: './logs-list.component.scss'
})
export class LogsListComponent {
    @Input() logs: LogEntry[] = [];
    @Input() pagination: any;
    @Output() pageChange = new EventEmitter<number>();
    @Output() sortChange = new EventEmitter<{ field: string; ascending: boolean }>();

    currentSortField = '';
    sortAscending = false;

    onSort(field: string) {
        if (this.currentSortField === field) {
            this.sortAscending = !this.sortAscending;
        } else {
            this.currentSortField = field;
            this.sortAscending = true;
        }
        this.sortChange.emit({field: this.currentSortField, ascending: this.sortAscending});
    }

    onPageChange(page: number) {
        this.pageChange.emit(page);
    }

    getSeverityClass(severity: string): 'success' | 'info' | 'warn' | 'danger' {
        switch (severity?.toLowerCase()) {
            case 'error':
                return 'danger';
            case 'warning':
                return 'warn';
            case 'info':
                return 'info';
            default:
                return 'success';
        }
    }

    getAdditionalFields(): string[] {
        if (this.logs.length === 0) return [];
        return Object.keys(this.logs[0].additionalFields || {});
    }
}
