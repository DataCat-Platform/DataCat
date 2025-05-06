import {Component, Input} from '@angular/core';
import {FormGroup, ReactiveFormsModule} from "@angular/forms";
import {Select} from "primeng/select";
import {InputText} from "primeng/inputtext";

@Component({
    selector: 'app-prometheus-settings-form',
    standalone: true,
    imports: [
        Select,
        ReactiveFormsModule,
        InputText
    ],
    templateUrl: './prometheus-settings-form.component.html',
    styleUrl: './prometheus-settings-form.component.scss'
})
export class PrometheusSettingsFormComponent {
    @Input() form!: FormGroup;
    @Input() authType!: string | undefined | null;

    authTypes = ['None', 'Basic', 'Bearer'];

    onAuthTypeChange() {
        const authType = this.form.value.authType;

        if (authType === 'None') {
            this.form.patchValue({
                username: null,
                password: null,
                authToken: null
            });
        }
    }
}
