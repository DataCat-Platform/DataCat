import {Component, EventEmitter, OnInit, Output} from '@angular/core';
import {FormControl, FormGroup, ReactiveFormsModule} from "@angular/forms";
import {DataSourcesFilter} from "../../../entities/data-sources/data-sources-filter";
import {catchError, debounceTime, finalize, tap} from "rxjs";
import {ToastLoggerService} from "../../../shared/services/toast-logger.service";
import {Select} from "primeng/select";
import {Panel} from "primeng/panel";
import {InputText} from "primeng/inputtext";
import {ProgressBar} from "primeng/progressbar";
import {
    ApiService,
    DataSourceKind,
    IGetDataSourceTypeResponse
} from "../../../shared/services/datacat-generated-client";

@Component({
    selector: 'app-data-sources-filtering',
    standalone: true,
    imports: [
        Select,
        Panel,
        InputText,
        ReactiveFormsModule,
        ProgressBar
    ],
    templateUrl: './data-sources-filtering.component.html',
    styleUrl: './data-sources-filtering.component.scss'
})
export class DataSourcesFilteringComponent implements OnInit {
    isBusy = false;

    @Output('filter') filterEmitter = new EventEmitter<DataSourcesFilter>();

    protected form = new FormGroup({
        name: new FormControl<string | null>(null),
        type: new FormControl<string | null>(null),
        typeId: new FormControl<number | null>(null),
        purpose: new FormControl<DataSourceKind | null>(null)
    });

    protected types: IGetDataSourceTypeResponse[] = [];
    protected purposes = Object.values(DataSourceKind);

    constructor(
        private apiService: ApiService,
        private toastLoggerService: ToastLoggerService,
    ) {
        this.form.valueChanges.pipe(
            debounceTime(300)
        ).subscribe((value) => {
            const selectedType = this.types.find(type => type.name === value.type);

            this.filterEmitter.emit({
                name: value.name || undefined,
                type: value.type || undefined,
                typeId: selectedType?.id || undefined,
                purpose: value.purpose || undefined
            });
        });
    }

    ngOnInit() {
        this.isBusy = true;

        this.apiService.getApiV1DataSourceTypeGetAll().pipe(
            finalize(() => this.isBusy = false),
            tap(res => {
                this.types = res;
            }),
            catchError((err) => {
                this.toastLoggerService.error(err);
                return err;
            })
        ).subscribe();
    }
}
