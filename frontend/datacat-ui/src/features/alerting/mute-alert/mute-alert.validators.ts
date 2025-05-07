import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

const DURATION_REGEX = /\d+[smhwMy]/g;

export const durationValidator: ValidatorFn = (
  control: AbstractControl,
): ValidationErrors | null => {
  const good = DURATION_REGEX.test(control.value);
  return good
    ? null
    : {
        value: control.value,
      };
};
