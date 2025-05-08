import {Component, OnInit} from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import {ApiService, SpanEntry, TraceEntry} from "../../shared/services/datacat-generated-client";
import {catchError, finalize, tap} from "rxjs";
import {ToastLoggerService} from "../../shared/services/toast-logger.service";
import {Button} from "primeng/button";
import {Panel} from "primeng/panel";
import {NgStyle} from "@angular/common";

@Component({
    selector: 'app-explore-trace-spans',
    standalone: true,
    imports: [
        Button,
        Panel,
        NgStyle
    ],
    templateUrl: './explore-trace-spans.component.html',
    styleUrl: './explore-trace-spans.component.scss'
})
export class ExploreTraceSpansComponent implements OnInit {
    isLoading = false;

    trace: TraceEntry | undefined;

    traceId: string = '';
    dataSourceName: string = '';

    // Добавляем новые свойства
    totalDurationMs = 0;
    startTime = 0;
    timeMarkers: number[] = [];

    constructor(
        private route: ActivatedRoute,
        private apiService: ApiService,
        private toastLoggerService: ToastLoggerService) {
    }

    ngOnInit(): void {
        this.traceId = this.route.snapshot.paramMap.get('traceId') || '';
        this.dataSourceName = this.route.snapshot.queryParamMap.get('dataSourceName') || '';

        if (!this.dataSourceName) {
            return;
        }

        this.isLoading = true;

        this.apiService.getApiV1Traces(this.traceId, this.dataSourceName).pipe(
            finalize(() => this.isLoading = false),
            tap(trace => {
                const processedSpans = trace.spans!
                    .filter(s => s.timestamp !== undefined && s.duration !== undefined)
                    .sort((a, b) => {
                            return a.timestamp! - b.timestamp!
                        }
                    );

                this.trace = {
                    ...trace,
                    spans: processedSpans
                } as TraceEntry;

                this.calculateTimeMetrics();
            }),
            catchError(error => {
                this.toastLoggerService.error(error.message);
                return error;
            })
        ).subscribe();
    }

    getSpanWidth(span: SpanEntry): number {
        const duration = this.parseDuration(span.duration!);
        return (duration / this.totalDurationMs) * 100;
    }

    getSpanOffset(span: SpanEntry): number {
        if (!span.startTime) return 0;

        const spanStart = new Date(span.startTime).getTime();
        const offset = spanStart - this.startTime;
        return Math.max(0, (offset / this.totalDurationMs) * 100);
    }

    formatTime(ms: number): string {
        if (this.totalDurationMs >= 1000) {
            const totalSeconds = ms / 1000;
            return `${totalSeconds.toFixed(2)}s`;
        }
        return `${ms}ms`;
    }

    parseDuration(duration: string): number {
        // example of input: "00:00:00.3090000"
        const parts = duration.split(':');
        const secondsPart = parts[2].split('.');

        const hours = parseInt(parts[0]) || 0;
        const minutes = parseInt(parts[1]) || 0;
        const seconds = parseInt(secondsPart[0]) || 0;
        const milliseconds = parseInt(secondsPart[1]?.substring(0, 3)) || 0; // Берем только первые 3 цифры

        return hours * 3600000 +
            minutes * 60000 +
            seconds * 1000 +
            milliseconds;
    }

    getServiceName(processId: string): string {
        return this.trace?.processes?.[processId]?.serviceName || 'Unknown';
    }

    private calculateTimeMetrics() {
        if (!this.trace?.spans?.length) return;

        const spans = this.trace.spans;

        const startTimestamps = spans.map(s => new Date(s.startTime!).getTime());
        this.startTime = Math.min(...startTimestamps);

        const endTimestamps = spans.map(s =>
            new Date(s.startTime!).getTime() + this.parseDuration(s.duration!)
        );
        this.totalDurationMs = Math.max(...endTimestamps) - this.startTime;

        this.timeMarkers = Array.from({length: 5}, (_, i) =>
            Math.round(this.startTime + (this.totalDurationMs * i) / 4)
        );
    }
}
