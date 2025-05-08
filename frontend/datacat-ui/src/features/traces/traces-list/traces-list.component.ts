import {Component, Input} from '@angular/core';
import {TraceEntry} from "../../../shared/services/datacat-generated-client";
import {TableModule} from "primeng/table";
import {RouterLink} from "@angular/router";
import {Tag} from "primeng/tag";
import {Button} from "primeng/button";
import {DatePipe, SlicePipe} from "@angular/common";
import {AppDialogService} from "../../../shared/services/app-dialog.service";
import {FormsModule} from "@angular/forms";

@Component({
    selector: 'app-traces-list',
    standalone: true,
    imports: [
        TableModule,
        RouterLink,
        Tag,
        Button,
        DatePipe,
        SlicePipe,
        FormsModule
    ],
    templateUrl: './traces-list.component.html',
    styleUrl: './traces-list.component.scss'
})
export class TracesListComponent {
    @Input() traces: TraceEntry[] = [];

    constructor(
        private appDialogService: AppDialogService) {
    }

    showDetails(log: TraceEntry) {
        // this.appDialogService.showDialog(TraceDetailsComponent, 'Trace Details', log);
    }

    getMaxDuration(): number {
        if (!this.traces || this.traces.length === 0) {
            return 0;
        }

        const durations = this.traces.flatMap(log =>
            log.spans?.map(span => this.parseDuration(span.duration || "00:00:00.0000000")) || []
        );

        return Math.max(...durations, 0);
    }

    getCurrentDuration(log: TraceEntry): number {
        const durations = log.spans?.map(span => this.parseDuration(span.duration || "00:00:00.0000000")) || [];
        return Math.max(...durations, 0);
    }

    private parseDuration(duration: string): number {
        const [time, milliseconds] = duration.split('.');
        const parts = time.split(':').map(Number);
        if (parts.length === 3) {
            const [hours, minutes, seconds] = parts;
            return (hours * 3600 + minutes * 60 + seconds) * 1000 + (parseInt(milliseconds) || 0);
        }
        return 0;
    };
}
