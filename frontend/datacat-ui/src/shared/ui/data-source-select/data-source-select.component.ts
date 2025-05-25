import { Component, forwardRef, Input } from '@angular/core';
import {
  ControlValueAccessor,
  FormsModule,
  NG_VALUE_ACCESSOR,
} from '@angular/forms';
import { SelectModule } from 'primeng/select';
import { DataSource, DataSourceDriver } from '../../../entities';
import { ScrollerOptions } from 'primeng/api';
import { ApiService } from '../../services/datacat-generated-client';
import { finalize } from 'rxjs';
import { ToastLoggerService } from '../../services/toast-logger.service';
import { DataSourcesFilter } from '../../../entities/data-sources/data-sources-filter';
import { convertToApiFilters } from '../../../entities/data-sources';

@Component({
  standalone: true,
  selector: 'datacat-data-source-select',
  templateUrl: './data-source-select.component.html',
  styleUrl: './data-source-select.component.scss',
  imports: [SelectModule, FormsModule],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => DataSourceSelectComponent),
      multi: true,
    },
  ],
})
export class DataSourceSelectComponent implements ControlValueAccessor {
  @Input() filter?: DataSourcesFilter;

  private onChange = (_: any) => {};
  private onTouched = () => {};

  protected DataSourceDriver = DataSourceDriver;

  protected dataSourcesPerPage = 5;
  protected isLoading = false;
  protected selectedDataSource?: DataSource;
  protected dataSources: DataSource[] = [];

  protected scrollerOptions: ScrollerOptions = {
    delay: 250,
    showLoader: true,
    lazy: true,
    onLazyLoad: this.onLazyLoad.bind(this),
  };

  constructor(
    private apiService: ApiService,
    private loggerService: ToastLoggerService,
  ) {}

  onLazyLoad(event: any) {
    this.isLoading = true;

    const { first, last } = event;

    const page = Math.floor(first / this.dataSourcesPerPage) + 1;

    const searchFilters = convertToApiFilters(this.filter);

    this.apiService
      .postApiV1DataSourceSearch(searchFilters, page, this.dataSourcesPerPage)
      .pipe(
        finalize(() => {
          this.isLoading = false;
        }),
      )
      .subscribe({
        next: (data) => {
          this.dataSources =
            data.items?.map<DataSource>((item) => {
              return {
                id: item.id || '',
                name: item.name || '',
                driver: item.type as DataSourceDriver,
                connectionUrl: item.connectionString || '',
              };
            }) || [];
        },
        error: () => {
          this.loggerService.error('Unable to load data sources');
        },
      });
  }

  writeValue(value: DataSource | undefined): void {
    this.selectedDataSource = value;
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  onSelect(dataSource: DataSource | undefined) {
    this.selectedDataSource = dataSource;
    this.onChange(dataSource);
    this.onTouched();
  }
}
