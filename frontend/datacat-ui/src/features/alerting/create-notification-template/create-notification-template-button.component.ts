import {Component} from '@angular/core';
import {Router} from '@angular/router';
import {ButtonModule} from 'primeng/button';
import * as urls from '../../../shared/common/urls';
import {timer} from 'rxjs';
import {DialogModule} from 'primeng/dialog';
import {InputTextModule} from 'primeng/inputtext';
import {FormControl, FormsModule, ReactiveFormsModule} from '@angular/forms';
import {ApiService} from "../../../shared/services/datacat-generated-client";

@Component({
    standalone: true,
    selector: './datacat-create-notification-template-button',
    templateUrl: './create-notification-template-button.component.html',
    styleUrl: './create-notification-template-button.component.scss',
    imports: [
        ButtonModule,
        InputTextModule,
        DialogModule,
        FormsModule,
        ReactiveFormsModule,
    ],
})
export class CreateNotificationTemplateButtonComponent {
    protected isCreationInitiated = false;
    protected isDialogVisible = false;

    protected templateName = new FormControl<string>('');

    constructor(
        private router: Router,
        private apiService: ApiService,
    ) {
    }

    protected createNotificationTemplate() {
        // TODO: call API
        this.templateName.disable();
        this.isCreationInitiated = true;
        timer(1000).subscribe({
            next: () => {
                const templateId = '0';
                this.router.navigateByUrl(urls.notificationTemplateEditUrl(templateId));
            },
            error: () => {
                // TODO
                this.templateName.enable();
                this.isCreationInitiated = false;
            },
        });
    }

    protected showCreationDialog() {
        this.isDialogVisible = true;
    }

    protected hideCreationDialog() {
        this.isDialogVisible = false;
    }
}
