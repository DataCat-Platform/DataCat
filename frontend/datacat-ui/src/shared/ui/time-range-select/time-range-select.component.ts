import { Component, forwardRef } from '@angular/core';
import {
  ControlValueAccessor,
  FormControl,
  FormGroup,
  NG_VALUE_ACCESSOR,
  ReactiveFormsModule,
} from '@angular/forms';
import { DatePickerModule } from 'primeng/datepicker';
import { TimeRange } from './time-range-select.types';
import { SelectModule } from 'primeng/select';
import { STEP_OPTIONS } from './time-range-select.consts';

@Component({
  standalone: true,
  selector: 'datacat-time-range-select',
  templateUrl: './time-range-select.component.html',
  styleUrl: './time-range-select.component.scss',
  imports: [DatePickerModule, SelectModule, ReactiveFormsModule],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => TimeRangeSelectComponent),
      multi: true,
    },
  ],
})
export class TimeRangeSelectComponent implements ControlValueAccessor {
  protected stepOptions = STEP_OPTIONS;

  private onChange = (_: any) => {};
  private onTouched = () => {};

  protected formGroup = new FormGroup({
    step: new FormControl<string>(this.stepOptions[0].value),
    from: new FormControl<Date>(new Date()),
    to: new FormControl<Date>(new Date()),
  });

  constructor() {
    this.formGroup.valueChanges.subscribe(() => this.notifyTouchedAndChanged());
  }

  writeValue(value: TimeRange | undefined): void {
    if (value) {
      this.formGroup.setValue(value);
    }
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  notifyTouchedAndChanged() {
    this.onChange(this.formGroup.getRawValue());
    this.onTouched();
  }
}
