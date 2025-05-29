import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { CheckboxModule } from 'primeng/checkbox';
import { SelectModule } from 'primeng/select';
import { ChartService } from '../../chart.service';

@Component({
  standalone: true,
  selector: 'datacat-tooltip-options',
  templateUrl: 'tooltip-options.component.html',
  styleUrl: '../options.style.scss',
  imports: [ReactiveFormsModule, CheckboxModule, SelectModule],
})
export class TooltipOptionsComponent {
  protected form = new FormGroup({
    enabled: new FormControl<boolean>(true),
  });

  constructor(private chartService: ChartService) {
    this.form.valueChanges.subscribe((options) =>
      this.chartService.updateStyle({
        tooltip: {
          ...options,
        },
      }),
    );
    this.chartService.style$.subscribe((style) => {
      if (style.tooltip) {
        this.form.setValue(style.tooltip, { emitEvent: false });
      }
    });
  }
}
