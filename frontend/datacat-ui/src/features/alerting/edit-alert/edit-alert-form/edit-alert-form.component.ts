import { Component, Input } from '@angular/core';
import { DividerModule } from 'primeng/divider';

import { CommonModule } from '@angular/common';
import { InputGroupModule } from 'primeng/inputgroup';
import { InputGroupAddonModule } from 'primeng/inputgroupaddon';
import { TagModule } from 'primeng/tag';
import { TooltipModule } from 'primeng/tooltip';
import { TextareaModule } from 'primeng/textarea';
import { InputTextModule } from 'primeng/inputtext';
import { InputNumberModule } from 'primeng/inputnumber';
import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';
import { SelectModule } from 'primeng/select';
import { PanelModule } from 'primeng/panel';
import { finalize } from 'rxjs';
import { ButtonModule } from 'primeng/button';
import { DataSource, NotificationGroup } from '../../../../entities';
import {
  ApiService,
  IUpdateAlertRequest,
} from '../../../../shared/services/datacat-generated-client';
import { ToastLoggerService } from '../../../../shared/services/toast-logger.service';
import { LoadingState } from '../../../../shared/common/enums';

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
  protected LoadingState = LoadingState;

  protected loadingState: LoadingState = LoadingState.Loading;
  protected isSaving = false;
  protected isError = false;

  protected dataSources?: DataSource[];
  protected notificationGroups?: NotificationGroup[];

  protected form = new FormGroup({
    description: new FormControl<string>(''),
    query: new FormControl<string>(''),
    executionInterval: new FormControl<string>(''),
    notificationTriggerPeriod: new FormControl<string>(''),
    notificationGroupId: new FormControl<string>(''),
    dataSourceId: new FormControl<string>(''),
  });

  @Input({ required: true }) set alertId(id: string) {
    this.loadEssentials(id);
  }

  protected get executionInterval() {
    return this.form.get('executionInterval')?.value;
  }

  protected get notificationTriggerPeriod() {
    return this.form.get('notificationTriggerPeriod')?.value;
  }

  constructor(
    private apiService: ApiService,
    private loggerService: ToastLoggerService,
  ) {}

  protected save() {
    this.isSaving = true;
    this.form.disable();
    const request: any = {} as IUpdateAlertRequest;
    this.apiService
      .putApiV1AlertUpdate(this.alertId, request)
      .pipe(
        finalize(() => {
          this.isSaving = false;
          this.form.enable();
        }),
      )
      .subscribe({
        next: () => {
          this.loggerService.success('Saved');
        },
        error: () => {
          this.loggerService.error('Unable to save alert');
        },
      });
  }

  private loadEssentials(alertId: string) {
    this.apiService
      .getApiV1Alert(alertId)
      .subscribe({
        next: (alert) => {
          this.form.setValue({
            description: alert.description || null,
            query: alert.rawQuery || null,
            executionInterval: alert.repeatInterval || null,
            notificationTriggerPeriod: alert.waitTimeBeforeAlerting || null,
            notificationGroupId: alert.notificationChannelGroup?.id || null,
            dataSourceId: alert.dataSource?.id || null,
          });
          this.loadingState = LoadingState.Success;
        },
        error: () => {
          this.loggerService.error('Unable to load alert data');
          this.loadingState = LoadingState.Error;
        },
      });

    this.apiService.getApiV1NotificationChannelGroupGetAll().subscribe({
      next: (dataSources) => {},
      error: () => {
        this.loggerService.error('Unable to load notification groups');
      },
    });
  }
}
