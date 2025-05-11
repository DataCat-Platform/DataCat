import { Component, Input } from '@angular/core';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { CheckboxModule } from 'primeng/checkbox';
import { SelectModule } from 'primeng/select';

@Component({
  standalone: true,
  selector: 'datacat-legend-options',
  templateUrl: './legend-options.component.html',
  styleUrl: '../styles.scss',
  imports: [ReactiveFormsModule, CheckboxModule, SelectModule],
})
export class LegendOptionsComponent {
  @Input() formGroup!: FormGroup;
}
