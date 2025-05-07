import {Component} from '@angular/core';
import {DynamicDialogConfig} from "primeng/dynamicdialog";
import {Dialog} from "primeng/dialog";
import {JsonPipe, NgForOf, NgIf} from "@angular/common";

@Component({
    selector: 'app-logs-details',
    standalone: true,
    imports: [
        Dialog,
        JsonPipe,
        NgForOf,
        NgIf
    ],
    templateUrl: './logs-details.component.html',
    styleUrl: './logs-details.component.scss'
})
export class LogsDetailsComponent {
    data: any;
    protected readonly Object = Object;

    constructor(
        public config: DynamicDialogConfig
    ) {
        this.data = config.data;
        console.log(this.data);
    }

    get objectKeys() {
        return Object.keys;
    }
}
