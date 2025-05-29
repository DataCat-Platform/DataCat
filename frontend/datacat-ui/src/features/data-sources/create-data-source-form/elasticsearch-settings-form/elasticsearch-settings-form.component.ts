import { Component, Input, OnInit } from '@angular/core';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { InputText } from 'primeng/inputtext';

@Component({
  selector: 'app-elasticsearch-settings-form',
  standalone: true,
  imports: [InputText, ReactiveFormsModule],
  templateUrl: './elasticsearch-settings-form.component.html',
  styleUrl: './elasticsearch-settings-form.component.scss',
})
export class ElasticsearchSettingsFormComponent {
  @Input() form!: FormGroup;
}
