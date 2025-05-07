import {Component, EventEmitter, Input, Output} from '@angular/core';
import {LogEntry} from "../../../shared/services/datacat-generated-client";
import {TableModule} from "primeng/table";
import {DatePipe, NgIf} from "@angular/common";
import {Tag} from "primeng/tag";
import {Button} from "primeng/button";
import {AppDialogService} from "../../../shared/services/app-dialog.service";
import {LogsDetailsComponent} from "../logs-details/logs-details.component";

@Component({
    selector: 'app-logs-list',
    standalone: true,
    imports: [
        TableModule,
        DatePipe,
        Tag,
        NgIf,
        Button
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

    constructor(
        private appDialogService: AppDialogService,
    ) {
    }

    onPageChange(event: any) {
        const newPage = (event.first / event.rows) + 1;
        this.pageChange.emit(newPage);
    }

    // Остальные методы без изменений
    onSort(field: string) {
        if (this.currentSortField === field) {
            this.sortAscending = !this.sortAscending;
        } else {
            this.currentSortField = field;
            this.sortAscending = true;
        }
        this.sortChange.emit({field: this.currentSortField, ascending: this.sortAscending});
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

    showDetails(additionalFields: any) {
        this.appDialogService.showDialog(LogsDetailsComponent,
            'Additional Fields',
            additionalFields || {}
        )
    }

    sanitizeMessage(message: string): string {
        try {
            if ((message.startsWith('"') && message.endsWith('"')) ||
                (message.startsWith('\"') && message.endsWith('\"'))) {
                message = message.slice(1, -1);
            }

            return JSON.parse(`"${message}"`);
        } catch {
            return message;
        }
    }
}
