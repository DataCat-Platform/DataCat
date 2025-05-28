import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { ChartService } from '../../chart.service';
import { SelectModule } from 'primeng/select';

@Component({
  standalone: true,
  selector: 'datacat-aggregate-options',
  templateUrl: 'aggregate-options.component.html',
  styleUrl: '../options.style.scss',
  imports: [ReactiveFormsModule, SelectModule],
})
export class AggregateOptionsComponent {
  protected form = new FormGroup({
    function: new FormControl<'AVG' | 'MIN' | 'MAX'>('AVG'),
  });

  constructor(private chartService: ChartService) {
    this.form.valueChanges.subscribe((options) =>
      this.chartService.updateStyle({
        aggregate: {
          ...options,
        },
      }),
    );
    this.chartService.style$.subscribe((style) => {
      if (style.aggregate) {
        this.form.setValue(style.aggregate, { emitEvent: false });
      }
    });
  }
}
