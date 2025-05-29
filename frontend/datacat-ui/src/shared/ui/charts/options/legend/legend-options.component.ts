import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { CheckboxModule } from 'primeng/checkbox';
import { SelectModule } from 'primeng/select';
import { ChartService } from '../../chart.service';

@Component({
  standalone: true,
  selector: 'datacat-legend-options',
  templateUrl: 'legend-options.component.html',
  styleUrl: '../options.style.scss',
  imports: [ReactiveFormsModule, CheckboxModule, SelectModule],
})
export class LegendOptionsComponent {
  protected form = new FormGroup({
    enabled: new FormControl<boolean>(true),
    position: new FormControl<'top' | 'bottom' | 'left' | 'right'>('top'),
  });

  constructor(private chartService: ChartService) {
    this.form.valueChanges.subscribe((options) =>
      this.chartService.updateStyle({
        legend: {
          ...options,
        },
      }),
    );
    this.chartService.style$.subscribe((style) => {
      if (style.legend) {
        this.form.setValue(style.legend, { emitEvent: false });
      }
    });
  }
}
