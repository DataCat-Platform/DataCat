import {Component, Input} from '@angular/core';
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
import {ApiService, IUpdateDataSourceRequest} from "../../../shared/services/datacat-generated-client";
import {ToastLoggerService} from "../../../shared/services/toast-logger.service";
import {catchError, finalize, tap} from "rxjs";
import {EditDataSource} from "../../../entities/data-sources/edit-data-source";

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
    @Input({required: true}) editDataSource: EditDataSource = null!;

    isBusy = false;

    form!: FormGroup;
    protected readonly FormGroup = FormGroup;

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
        console.log(this.editDataSource);

        this.form = this.fb.group({
            name: [this.editDataSource?.name, Validators.required],
            settings: this.fb.group({})
        });

        this.form.get('name')?.disable();

        this.updateSettingsForm(this.editDataSource.type!);
    }

    saveDataSource() {
        if (this.form.invalid) return;

        const formValue = this.form.value;
        const connectionSettings = this.getConnectionSettings(formValue);

        const request: any = {
            connectionString: JSON.stringify(connectionSettings)
        } as IUpdateDataSourceRequest;

        this.apiService.putApiV1DataSourceUpdateConnectionString(this.editDataSource.id, request).pipe(
            finalize(() => this.isBusy = false),
            tap(_ => {
                this.toastLoggerService.info('Data Source updated successfully');
            }),
            catchError((err) => {
                this.toastLoggerService.error(err);
                return err;
            })
        ).subscribe()
    }

    private updateSettingsForm(type: string) {
        Object.keys(this.settingsForm.controls).forEach(key => {
            this.settingsForm.removeControl(key);
        });

        let parsedSettings: Record<string, any> = {};
        try {
            parsedSettings = this.editDataSource?.connectionString
                ? JSON.parse(this.editDataSource.connectionString)
                : {};
        } catch (e) {
            console.warn('Failed to parse connection string:', e);
        }

        const regexp_no_domain_validation = /^(https?:\/\/)?((([a-z0-9-]+\.)+([a-z]{2,}))|((\d{1,3}\.){3}\d{1,3}))(:\d{1,5})?$/i;
        const regexp_with_domain_validation = /^(https?:\/\/)?((((?!-)([a-z0-9-]{1,63})(?<!-)\.)+([a-z]{2,}))|((\d{1,3}\.){3}\d{1,3}))(:\d{1,5})?$/i;

        const urlPattern = Validators.pattern(regexp_no_domain_validation);
        const urlPattern_2 = Validators.pattern(regexp_with_domain_validation);

        switch (type) {
            case 'prometheus':
                this.settingsForm.addControl('serverUrl', this.fb.control(
                    parsedSettings['serverUrl'],
                    [Validators.required, urlPattern, urlPattern_2]
                ));
                this.settingsForm.addControl('authType', this.fb.control(parsedSettings['authType'] ?? 'None'));
                this.settingsForm.addControl('username', this.fb.control(parsedSettings['username'] ?? ''));
                this.settingsForm.addControl('password', this.fb.control(parsedSettings['password'] ?? ''));
                this.settingsForm.addControl('authToken', this.fb.control(parsedSettings['authToken'] ?? ''));
                break;

            case 'jaeger':
                this.settingsForm.addControl('serverUrl', this.fb.control(
                    parsedSettings['serverUrl'],
                    [Validators.required, urlPattern, urlPattern_2]
                ));
                this.settingsForm.addControl('authType', this.fb.control(parsedSettings['authType'] ?? 'None'));
                this.settingsForm.addControl('username', this.fb.control(parsedSettings['username'] ?? ''));
                this.settingsForm.addControl('password', this.fb.control(parsedSettings['password'] ?? ''));
                this.settingsForm.addControl('authToken', this.fb.control(parsedSettings['authToken'] ?? ''));
                break;

            case 'elasticsearch':
                this.settingsForm.addControl('clusterUrl', this.fb.control(
                    parsedSettings['clusterUrl'],
                    [Validators.required, urlPattern, urlPattern_2]
                ));
                this.settingsForm.addControl('indexPattern', this.fb.control(parsedSettings['indexPattern'], Validators.required));
                this.settingsForm.addControl('requestTimeout', this.fb.control(parsedSettings['requestTimeout'], [Validators.min(1), Validators.max(86400)]));
                this.settingsForm.addControl('userName', this.fb.control(parsedSettings['userName']));
                this.settingsForm.addControl('password', this.fb.control(parsedSettings['password']));
                this.settingsForm.addControl('enableDebugLogging', this.fb.control(parsedSettings['enableDebugLogging']));
                break;
        }
    }

    private getConnectionSettings(formValue: any): any {
        const settings = formValue.settings || {};
        const type = this.editDataSource.type;

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
