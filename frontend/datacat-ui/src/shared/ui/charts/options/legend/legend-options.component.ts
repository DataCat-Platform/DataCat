import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { StyleService } from '../../style.service';
import { CheckboxModule } from 'primeng/checkbox';
import { SelectModule } from 'primeng/select';

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

  constructor(private styleService: StyleService) {
    this.form.valueChanges.subscribe((options) =>
      this.styleService.updateStyle(options),
    );
  }
}
