import {Component, OnInit} from '@angular/core';
import {Panel} from "primeng/panel";
import {ActivatedRoute} from "@angular/router";
import {ApiService, GetDataSourceResponse} from "../../shared/services/datacat-generated-client";
import {ToastLoggerService} from "../../shared/services/toast-logger.service";
import {catchError, finalize, tap} from "rxjs";
import {Divider} from "primeng/divider";
import {PrimeTemplate} from "primeng/api";
import {ProgressBar} from "primeng/progressbar";
import {
    DeleteDataSourceButtonComponent
} from "../../features/data-sources/delete-data-source-button/delete-data-source-button.component";

@Component({
    selector: 'app-manage-data-sources',
    standalone: true,
    imports: [
        Panel,
        Divider,
        PrimeTemplate,
        ProgressBar,
        DeleteDataSourceButtonComponent
    ],
    templateUrl: './manage-data-sources.component.html',
    styleUrl: './manage-data-sources.component.scss'
})
export class ManageDataSourcesComponent implements OnInit {
    isLoading = false;
    dataSourceId!: string;
    dataSource!: GetDataSourceResponse;

    constructor(
        private route: ActivatedRoute,
        private apiService: ApiService,
        private toastLogger: ToastLoggerService
    ) {
    }

    ngOnInit(): void {
        this.loadDataSource();
    }

    private loadDataSource(): void {
        this.isLoading = true;
        this.dataSourceId = this.route.snapshot.params['data-source-id'];

        this.apiService.getApiV1DataSource(this.dataSourceId).pipe(
            tap((data: GetDataSourceResponse) => {
                this.dataSource = data;
            }),
            catchError((err) => {
                this.toastLogger.error(err.message);
                throw err;
            }),
            finalize(() => {
                this.isLoading = false;
            })
        ).subscribe();
    }
}
