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
import { PopoverModule } from 'primeng/popover';
import { ButtonModule } from 'primeng/button';
import { CommonModule } from '@angular/common';

@Component({
  standalone: true,
  selector: 'datacat-time-range-select',
  templateUrl: './time-range-select.component.html',
  styleUrl: './time-range-select.component.scss',
  imports: [
    DatePickerModule,
    SelectModule,
    ReactiveFormsModule,
    PopoverModule,
    ButtonModule,
    CommonModule,
  ],
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
    step: new FormControl<string | null>(null),
    from: new FormControl<Date | null>(null),
    to: new FormControl<Date | null>(null),
  });

  public get step(): string {
    return this.formGroup.get('step')?.value!;
  }

  public get from(): Date {
    return this.formGroup.get('from')?.value!;
  }

  public get to(): Date {
    return this.formGroup.get('to')?.value!;
  }

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
