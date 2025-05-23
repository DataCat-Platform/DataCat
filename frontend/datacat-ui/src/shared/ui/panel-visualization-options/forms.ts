import { FormControl, FormGroup } from '@angular/forms';
import { Position, VisualizationType } from '../../../entities';

export const createOptionsForm = (
  type: VisualizationType | null,
): FormGroup<any> => {
  return new FormGroup({
    legend: new FormGroup({
      enabled: new FormControl<boolean>(false),
      position: new FormControl<Position>('top'),
    }),
    title: new FormGroup({
      enabled: new FormControl<boolean>(true),
      text: new FormControl<string>('a'),
    }),
    tooltip: new FormGroup({
      enabled: new FormControl<boolean>(true),
    })
  });

  // switch (type) {
  //   case VisualizationType.LINE: {
  //   }
  //   default: {
  //     return new FormGroup({});
  //   }
  // }
};
