import {Component, Input} from '@angular/core';
import {DividerModule} from 'primeng/divider';

import {CommonModule} from '@angular/common';
import {InputGroupModule} from 'primeng/inputgroup';
import {InputGroupAddonModule} from 'primeng/inputgroupaddon';
import {TagModule} from 'primeng/tag';
import {TooltipModule} from 'primeng/tooltip';
import {TextareaModule} from 'primeng/textarea';
import {InputTextModule} from 'primeng/inputtext';
import {InputNumberModule} from 'primeng/inputnumber';
import {FormControl, FormGroup, FormsModule, ReactiveFormsModule,} from '@angular/forms';
import {SelectModule} from 'primeng/select';
import {PanelModule} from 'primeng/panel';
import {from, timer} from 'rxjs';
import {FAKE_ALERT, FAKE_DATASOURCE, FAKE_NOTIFICATION_GROUP,} from '../../../../shared/mock/fakes';
import {ButtonModule} from 'primeng/button';
import {Alert, DataSource, NotificationGroup} from "../../../../entities";
import {ApiService} from "../../../../shared/services/datacat-generated-client";

@Component({
    standalone: true,
    selector: 'datacat-edit-alert-form',
    templateUrl: './edit-alert-form.component.html',
    styleUrl: './edit-alert-form.component.scss',
    imports: [
        PanelModule,
        DividerModule,
        CommonModule,
        InputGroupModule,
        InputGroupAddonModule,
        TagModule,
        TooltipModule,
        TextareaModule,
        InputTextModule,
        InputNumberModule,
        FormsModule,
        SelectModule,
        ReactiveFormsModule,
        ButtonModule,
    ],
})
export class EditAlertFormComponent {
    protected isSaving = false;
    protected alert?: Alert;
    protected dataSources?: DataSource[];
    protected notificationGroups?: NotificationGroup[];
    protected form = new FormGroup({
        description: new FormControl<string>(''),
        query: new FormControl<string>(''),
        executionInterval: new FormControl<number>(0),
        notificationTriggerPeriod: new FormControl<number>(0),
        notificationGroupId: new FormControl<string>(''),
        dataSourceId: new FormControl<string>(''),
    });

    constructor(private apiService: ApiService) {
    }

    @Input({required: true}) set alertId(id: string) {
        this.loadEssentials(id);
    }

    protected get executionInterval() {
        return this.form.get('executionInterval')?.value;
    }

    protected get notificationTriggerPeriod() {
        return this.form.get('notificationTriggerPeriod')?.value;
    }

    protected save() {
        // TODO: call API
        this.isSaving = true;
        this.form.disable();
        timer(2000).subscribe({
            next: () => {
                this.isSaving = false;
                this.form.enable();
            },
            error: () => {
                // TODO
                this.form.enable();
            },
        });
    }

    private loadEssentials(alertId: string) {
        // TODO: call API

        from([[FAKE_DATASOURCE]]).subscribe({
            next: (dataSources) => (this.dataSources = dataSources),
            error: () => {
                // TODO
            },
        });

        from([[FAKE_NOTIFICATION_GROUP]]).subscribe({
            next: (notificationGroups) =>
                (this.notificationGroups = notificationGroups),
            error: () => {
                // TODO
            },
        });

        from([FAKE_ALERT]).subscribe({
            next: (alert) => {
                this.alert = alert;

                this.form.get('description')?.setValue(this.alert.description);
                this.form.get('query')?.setValue(this.alert.query);
                this.form
                    .get('executionInterval')
                    ?.setValue(this.alert.executionInterval);
                this.form
                    .get('notificationTriggerPeriod')
                    ?.setValue(this.alert.notificationTriggerPeriod);
                this.form.get('dataSourceId')?.setValue(this.alert.dataSourceId);
                this.form
                    .get('notificationGroupId')
                    ?.setValue(this.alert.notificationGroupId);
            },
            error: () => {
                // TODO
            },
        });
    }
}
