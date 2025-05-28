import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { CheckboxModule } from 'primeng/checkbox';
import { ChartService } from '../../chart.service';
import { InputTextModule } from 'primeng/inputtext';
import { DatePickerModule } from 'primeng/datepicker';
import { InputNumberModule } from 'primeng/inputnumber';

@Component({
  standalone: true,
  selector: 'datacat-axis-options',
  templateUrl: 'axis-options.component.html',
  styleUrl: '../options.style.scss',
  imports: [
    ReactiveFormsModule,
    CheckboxModule,
    InputTextModule,
    InputNumberModule,
    DatePickerModule,
  ],
})
export class AxisOptionsComponent {
  protected form = new FormGroup({
    xAxisTitle: new FormControl<string | null>(null),
    xAxisMin: new FormControl<Date | null>(null),
    xAxisMax: new FormControl<Date | null>(null),
    yAxisTitle: new FormControl<string | null>(null),
    yAxisMin: new FormControl<number | null>(null),
    yAxisMax: new FormControl<number | null>(null),
  });

  constructor(private chartService: ChartService) {
    this.form.valueChanges.subscribe((options) =>
      this.chartService.updateStyle({
        axis: {
          ...options,
        },
      }),
    );
    this.chartService.style$.subscribe((style) => {
      if (style.axis) {
        this.form.setValue(style.axis, { emitEvent: false });
      }
    });
  }
}
