import {Injectable, Type} from '@angular/core';
import {DialogService, DynamicDialogRef} from "primeng/dynamicdialog";

@Injectable({
    providedIn: 'root'
})
export class AppDialogService {

    ref: DynamicDialogRef | undefined;

    constructor(public dialogService: DialogService) {
    }

    showDialog(
        component: Type<any>,
        header: string,
        data: any,
    ): void {
        this.ref = this.dialogService.open(component, {
            header: `${header}`,
            data: data,
            modal: true,
            width: '1200px',
            height: '800px',
            focusOnShow: false,
        });
    }
}
