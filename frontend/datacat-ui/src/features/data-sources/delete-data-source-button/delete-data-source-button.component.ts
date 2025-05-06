import {Component, Input} from '@angular/core';
import {Button} from "primeng/button";
import {Dialog} from "primeng/dialog";
import {ApiService} from "../../../shared/services/datacat-generated-client";
import {Router} from "@angular/router";
import {catchError, tap} from "rxjs";
import * as urls from "../../../shared/common/urls";
import {ToastLoggerService} from "../../../shared/services/toast-logger.service";

@Component({
    selector: 'app-delete-data-source-button',
    standalone: true,
    imports: [
        Button,
        Dialog
    ],
    templateUrl: './delete-data-source-button.component.html',
    styleUrl: './delete-data-source-button.component.scss'
})
export class DeleteDataSourceButtonComponent {
    @Input({required: true}) dataSourceId?: string;

    protected isDeletionInitiated = false;
    protected isDeletionDialogVisible = false;
    protected isDeletionError = false;

    constructor(
        private apiService: ApiService,
        private router: Router,
        private toastLoggerService: ToastLoggerService,
    ) {
    }

    protected showDeletionDialog() {
        this.isDeletionError = false;
        this.isDeletionDialogVisible = true;
    }

    protected hideDeletionDialog() {
        this.isDeletionDialogVisible = false;
    }

    protected deleteNotificationGroup() {
        this.isDeletionError = false;
        this.isDeletionInitiated = true;
        if (this.dataSourceId) {
            this.apiService.deleteApiV1DataSourceRemove(this.dataSourceId).pipe(
                tap(_ => {
                    this.toastLoggerService.success('Data Source successfully deleted');
                    this.router.navigateByUrl(urls.DATA_SOURCES_EXPLORER_URL);
                }),
                catchError(err => {
                    this.isDeletionInitiated = false;
                    this.isDeletionError = true;
                    return err;
                })
            ).subscribe()
        }
    }
}
