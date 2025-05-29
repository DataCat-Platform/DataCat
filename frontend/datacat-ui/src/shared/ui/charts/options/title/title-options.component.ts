import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { CheckboxModule } from 'primeng/checkbox';
import { ChartService } from '../../chart.service';
import { InputTextModule } from 'primeng/inputtext';

@Component({
  standalone: true,
  selector: 'datacat-title-options',
  templateUrl: 'title-options.component.html',
  styleUrl: '../options.style.scss',
  imports: [ReactiveFormsModule, CheckboxModule, InputTextModule],
})
export class TitleOptionsComponent {
  protected form = new FormGroup({
    enabled: new FormControl<boolean>(true),
    text: new FormControl<string>(''),
  });

  constructor(private chartService: ChartService) {
    this.form.valueChanges.subscribe((options) =>
      this.chartService.updateStyle({
        title: {
          ...options,
        },
      }),
    );
    this.chartService.style$.subscribe((style) => {
      if (style.title) {
        this.form.setValue(style.title, { emitEvent: false });
      }
    });
  }
}
