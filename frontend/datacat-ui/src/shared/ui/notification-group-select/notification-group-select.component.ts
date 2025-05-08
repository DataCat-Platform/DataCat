import { Component, forwardRef } from '@angular/core';
import {
  ControlValueAccessor,
  FormsModule,
  NG_VALUE_ACCESSOR,
} from '@angular/forms';
import { SelectModule } from 'primeng/select';
import { DataSourceDriver, NotificationGroupExpanded } from '../../../entities';
import { ApiService } from '../../services/datacat-generated-client';
import { finalize } from 'rxjs';
import { ToastLoggerService } from '../../services/toast-logger.service';

@Component({
  standalone: true,
  selector: 'datacat-notification-group-select',
  templateUrl: './notification-group-select.component.html',
  imports: [SelectModule, FormsModule],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => NotificationGroupSelectComponent),
      multi: true,
    },
  ],
})
export class NotificationGroupSelectComponent implements ControlValueAccessor {
  private onChange = (_: any) => {};
  private onTouched = () => {};

  protected DataSourceDriver = DataSourceDriver;

  protected isLoading = false;
  protected selectedNotificationGroup?: NotificationGroupExpanded;
  protected notificationGroups: NotificationGroupExpanded[] = [];

  constructor(
    private apiService: ApiService,
    private loggerService: ToastLoggerService,
  ) {}

  onLazyLoad(event: any) {
    this.isLoading = true;

    this.apiService
      .getApiV1NotificationChannelGroupGetAll()
      .pipe(
        finalize(() => {
          this.isLoading = false;
        }),
      )
      .subscribe({
        next: (data) => {
          this.notificationGroups =
            data.map<NotificationGroupExpanded>((item) => {
              return {
                id: item.id || '',
                name: item.name || '',
                notificationChannels: [],
              };
            }) || [];
        },
        error: () => {
          this.loggerService.error('Unable to load notification channels');
        },
      });
  }

  writeValue(value: NotificationGroupExpanded | undefined): void {
    this.selectedNotificationGroup = value;
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  onSelect(notificationGroup: NotificationGroupExpanded | undefined) {
    this.selectedNotificationGroup = notificationGroup;
    this.onChange(notificationGroup);
    this.onTouched();
  }
}
