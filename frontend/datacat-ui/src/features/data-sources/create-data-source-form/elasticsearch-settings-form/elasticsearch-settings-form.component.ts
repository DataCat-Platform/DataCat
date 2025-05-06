import {Component, Input, OnInit} from '@angular/core';
import {FormGroup, ReactiveFormsModule} from "@angular/forms";
import {InputNumber} from "primeng/inputnumber";
import {InputText} from "primeng/inputtext";
import {Checkbox} from "primeng/checkbox";

@Component({
    selector: 'app-elasticsearch-settings-form',
    standalone: true,
    imports: [
        InputNumber,
        InputText,
        ReactiveFormsModule,
        Checkbox
    ],
    templateUrl: './elasticsearch-settings-form.component.html',
    styleUrl: './elasticsearch-settings-form.component.scss'
})
export class ElasticsearchSettingsFormComponent implements OnInit {
    @Input() form!: FormGroup;

    ngOnInit() {
    }
}
