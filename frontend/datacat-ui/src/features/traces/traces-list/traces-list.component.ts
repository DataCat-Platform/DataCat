import {Component, Input} from '@angular/core';
import {TraceEntry} from "../../../shared/services/datacat-generated-client";
import {TableModule} from "primeng/table";
import {Router} from "@angular/router";
import {Tag} from "primeng/tag";
import {DatePipe, SlicePipe} from "@angular/common";
import {FormsModule} from "@angular/forms";
import * as urls from "../../../shared/common/urls";

@Component({
    selector: 'app-traces-list',
    standalone: true,
    imports: [
        TableModule,
        Tag,
        SlicePipe,
        FormsModule,
        DatePipe
    ],
    templateUrl: './traces-list.component.html',
    styleUrl: './traces-list.component.scss'
})
export class TracesListComponent {
    @Input() traces: TraceEntry[] = [];

    constructor(
        private router: Router) {
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

    getSpanCount(trace: TraceEntry): number {
        if (trace?.spans && Array.isArray(trace.spans)) {
            return trace.spans.length;
        }
        return 0;
    }

    getTraceServices(trace: TraceEntry): string[] {
        if (!trace?.processes) return [];

        const services = new Set<string>();
        for (const processId in trace.processes) {
            const process = trace.processes[processId];
            if (process?.serviceName) {
                services.add(process.serviceName);
            }
        }

        return Array.from(services);
    }

    getTracesStartTime(trace: TraceEntry): string {
        const minStartTime = trace.spans?.reduce((minTime, span) => {
            const spanStartTime = new Date(span.startTime!);
            return spanStartTime < minTime ? spanStartTime : minTime;
        }, new Date());

        return minStartTime?.toISOString() || "";
    }

    getTracesTimeAgo(trace: TraceEntry): string {
        if (!trace.spans || trace.spans.length === 0) {
            return 'No traces available';
        }

        const traceStartTime = this.getTracesStartTime(trace);
        if (!traceStartTime) {
            return "";
        }

        const now = new Date();
        const diffMs = now.getTime() - new Date(traceStartTime).getTime();
        const diffSec = Math.floor(diffMs / 1000);
        const diffMin = Math.floor(diffSec / 60);
        const diffHour = Math.floor(diffMin / 60);

        if (diffHour > 0) {
            return `${diffHour} hour${diffHour > 1 ? 's' : ''} ago`;
        } else if (diffMin > 0) {
            return `${diffMin} minute${diffMin > 1 ? 's' : ''} ago`;
        } else {
            return `${diffSec} second${diffSec !== 1 ? 's' : ''} ago`;
        }
    }

    getFormattedDuration(durationMs: number): string {
        if (durationMs >= 3600000) {
            return `${(durationMs / 3600000).toFixed(2)} hour${durationMs / 3600000 > 1 ? 's' : ''}`;
        } else if (durationMs >= 1000) {
            return `${(durationMs / 1000).toFixed(2)} second${durationMs / 1000 > 1 ? 's' : ''}`;
        } else {
            return `${durationMs}ms`;
        }
    }

    parseDuration(duration: string): number {
        const regex = /(\d{2}):(\d{2}):(\d{2})\.(\d{7})/;
        const match = duration.match(regex);

        if (match) {
            const hours = parseInt(match[1], 10);
            const minutes = parseInt(match[2], 10);
            const seconds = parseInt(match[3], 10);
            const milliseconds = parseInt(match[4], 10) / 10000;

            return hours * 3600000 + minutes * 60000 + seconds * 1000 + milliseconds;
        }

        return 0;
    }

    redirectToTraceDetails(traceId: string) {
        this.router.navigateByUrl(urls.traceUrl(traceId))
    }
}
