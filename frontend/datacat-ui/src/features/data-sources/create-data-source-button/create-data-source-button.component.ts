import {Component} from '@angular/core';
import {Router} from "@angular/router";
import {ApiService} from "../../../shared/services/api.service";
import {ButtonModule} from "primeng/button";
import {AppDialogService} from "../../../shared/services/app-dialog.service";
import {CreateDataSourceFormComponent} from "../create-data-source-form/create-data-source-form.component";

@Component({
    selector: 'app-create-data-source-button',
    standalone: true,
    imports: [
        ButtonModule
    ],
    templateUrl: './create-data-source-button.component.html',
    styleUrl: './create-data-source-button.component.scss'
})
export class CreateDataSourceButtonComponent {
    protected isBusy = false;

    constructor(
        private dialogService: AppDialogService,
        private router: Router,
        private apiService: ApiService,
    ) {
    }

    protected createDataSource() {
        this.dialogService.showDialog(CreateDataSourceFormComponent, 'Create Data Source', {});

        // // TODO: call API
        // timer(1000).subscribe({
        //     next: () => {
        //         const alertId = '0';
        //         return this.router.navigateByUrl(urls.alertEditUrl(alertId));
        //     },
        //     error: () => {
        //         // TODO
        //         this.isBusy = false;
        //     },
        // });
    }
}
