import { Component, Input } from '@angular/core';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { CheckboxModule } from 'primeng/checkbox';
import { InputTextModule } from 'primeng/inputtext';

@Component({
  standalone: true,
  selector: 'datacat-title-options',
  templateUrl: './title-options.component.html',
  styleUrl: '../styles.scss',
  imports: [ReactiveFormsModule, CheckboxModule, InputTextModule],
})
export class TitleOptionsComponent {
  @Input() formGroup!: FormGroup;
}
