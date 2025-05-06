import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {ButtonModule} from "primeng/button";
import {InputTextModule} from "primeng/inputtext";
import {SelectModule} from "primeng/select";
import {
    ApiService,
    DataSourceKind,
    IAddDataSourceRequest,
    IGetDataSourceTypeResponse
} from "../../../app/core/services/datacat-generated-client";
import {PrometheusSettingsFormComponent} from "./prometheus-settings-form/prometheus-settings-form.component";
import {catchError, finalize, tap} from "rxjs";
import {ToastLoggerService} from "../../../shared/services/toast-logger.service";
import {JaegerSettingsFormComponent} from "./jaeger-settings-form/jaeger-settings-form.component";
import {ElasticsearchSettingsFormComponent} from "./elasticsearch-settings-form/elasticsearch-settings-form.component";

@Component({
    selector: 'app-create-data-source-form',
    standalone: true,
    imports: [
        ReactiveFormsModule,
        ButtonModule,
        InputTextModule,
        SelectModule,
        PrometheusSettingsFormComponent,
        JaegerSettingsFormComponent,
        ElasticsearchSettingsFormComponent
    ],
    templateUrl: './create-data-source-form.component.html',
    styleUrl: './create-data-source-form.component.scss'
})
export class CreateDataSourceFormComponent implements OnInit {
    isBusy = false;

    dataSourceTypes: IGetDataSourceTypeResponse[] = [];
    dataSourcePurposes = Object.values(DataSourceKind);

    form = this.fb.group({
        name: ['', Validators.required],
        dataSourceType: ['', Validators.required],
        purpose: [null as DataSourceKind | null, Validators.required],
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

        this.apiService.getApiV1DataSourceTypeGetAll().pipe(
            finalize(() => this.isBusy = false),
            tap(res => {
                this.dataSourceTypes = res;
            }),
            catchError((err) => {
                this.toastLoggerService.error(err);
                return err;
            })
        ).subscribe();

        this.form.get('dataSourceType')?.valueChanges.subscribe(type => {
            this.updateSettingsForm(type!);
        });
    }

    saveDataSource() {
        if (this.form.invalid) return;

        const formValue = this.form.value;
        const connectionSettings = this.getConnectionSettings(formValue);

        const request: any = {
            uniqueName: formValue.name!,
            dataSourceType: formValue.dataSourceType!,
            purpose: formValue.purpose!,
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
