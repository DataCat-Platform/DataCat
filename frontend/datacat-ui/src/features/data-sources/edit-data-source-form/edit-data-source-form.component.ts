import {Component} from '@angular/core';
import {Button} from "primeng/button";
import {
    ElasticsearchSettingsFormComponent
} from "../create-data-source-form/elasticsearch-settings-form/elasticsearch-settings-form.component";
import {InputText} from "primeng/inputtext";
import {
    JaegerSettingsFormComponent
} from "../create-data-source-form/jaeger-settings-form/jaeger-settings-form.component";
import {
    PrometheusSettingsFormComponent
} from "../create-data-source-form/prometheus-settings-form/prometheus-settings-form.component";
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {ApiService, IAddDataSourceRequest} from "../../../shared/services/datacat-generated-client";
import {ToastLoggerService} from "../../../shared/services/toast-logger.service";
import {catchError, finalize, tap} from "rxjs";

@Component({
    selector: 'app-edit-data-source-form',
    standalone: true,
    imports: [
        Button,
        ElasticsearchSettingsFormComponent,
        InputText,
        JaegerSettingsFormComponent,
        PrometheusSettingsFormComponent,
        ReactiveFormsModule
    ],
    templateUrl: './edit-data-source-form.component.html',
    styleUrl: './edit-data-source-form.component.scss'
})
export class EditDataSourceFormComponent {
    isBusy = false;

    form = this.fb.group({
        name: ['', Validators.required],
        settings: this.fb.group({})
    });

    constructor(
        private fb: FormBuilder,
        private apiService: ApiService,
        private toastLoggerService: ToastLoggerService
    ) {
    }

    get settingsForm(): FormGroup {
        return this.form.get('settings') as FormGroup;
    }

    ngOnInit() {
        this.isBusy = true;
    }

    saveDataSource() {
        if (this.form.invalid) return;

        const formValue = this.form.value;
        const connectionSettings = this.getConnectionSettings(formValue);

        const request: any = {
            uniqueName: formValue.name!,
            connectionString: JSON.stringify(connectionSettings)
        } as IAddDataSourceRequest;

        this.apiService.postApiV1DataSourceAdd(request).pipe(
            finalize(() => this.isBusy = false),
            tap(res => {
                this.toastLoggerService.info(res);
            }),
            catchError((err) => {
                this.toastLoggerService.error(err);
                return err;
            })
        ).subscribe()
    }

    private updateSettingsForm(type: string) {
        while (Object.keys(this.settingsForm.controls).length) {
            this.settingsForm.removeControl(Object.keys(this.settingsForm.controls)[0]);
        }

        const regexp_no_domain_validation = /^(https?:\/\/)?((([a-z0-9-]+\.)+([a-z]{2,}))|((\d{1,3}\.){3}\d{1,3}))(:\d{1,5})?$/i;
        const regexp_with_domain_validation = /^(https?:\/\/)?((((?!-)([a-z0-9-]{1,63})(?<!-)\.)+([a-z]{2,}))|((\d{1,3}\.){3}\d{1,3}))(:\d{1,5})?$/i;

        const urlPattern = Validators.pattern(regexp_no_domain_validation);
        const urlPattern_2 = Validators.pattern(regexp_with_domain_validation);

        switch (type) {
            case 'prometheus':
                this.settingsForm.addControl('serverUrl', this.fb.control('https://', [Validators.required, urlPattern, urlPattern_2]));
                this.settingsForm.addControl('authType', this.fb.control('None'));
                this.settingsForm.addControl('username', this.fb.control(''));
                this.settingsForm.addControl('password', this.fb.control(''));
                this.settingsForm.addControl('authToken', this.fb.control(''));
                break;
            case 'jaeger':
                this.settingsForm.addControl('serverUrl', this.fb.control('https://', [Validators.required, urlPattern, urlPattern_2]));
                this.settingsForm.addControl('authType', this.fb.control('None'));
                this.settingsForm.addControl('username', this.fb.control(''));
                this.settingsForm.addControl('password', this.fb.control(''));
                this.settingsForm.addControl('authToken', this.fb.control(''));
                break;
            case 'elasticsearch':
                this.settingsForm.addControl('clusterUrl', this.fb.control('https://', [Validators.required, urlPattern, urlPattern_2]));
                this.settingsForm.addControl('indexPattern', this.fb.control('', Validators.required));
                this.settingsForm.addControl('requestTimeout', this.fb.control(30, [Validators.min(1), Validators.max(86400)]));
                this.settingsForm.addControl('userName', this.fb.control('', []));
                this.settingsForm.addControl('password', this.fb.control('', []));
                this.settingsForm.addControl('enableDebugLogging', this.fb.control(false));
                break;
        }
    }

    private getConnectionSettings(formValue: any): any {
        const settings = formValue.settings || {};
        const type = formValue.dataSourceType;

        switch (type) {
            case 'prometheus':
                return {
                    serverUrl: settings.serverUrl,
                    authType: settings.authType,
                    username: settings.username,
                    password: settings.password,
                    authToken: settings.authToken
                };
            case 'jaeger':
                return {
                    serverUrl: settings.serverUrl,
                    authType: settings.authType,
                    username: settings.username,
                    password: settings.password,
                    authToken: settings.authToken
                };
            case 'elasticsearch':
                return {
                    clusterUrl: settings.clusterUrl,
                    indexPattern: settings.indexPattern,
                    requestTimeout: settings.requestTimeout,
                    userName: settings.userName,
                    password: settings.password,
                    enableDebugLogging: settings.enableDebugLogging
                };
        }
    }
}
