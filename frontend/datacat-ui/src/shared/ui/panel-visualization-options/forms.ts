import { FormControl, FormGroup } from '@angular/forms';
import { Position, VisualizationType } from '../../../entities';

export const createOptionsForm = (type: VisualizationType | null) => {
  switch (type) {
    case VisualizationType.LINE: {
      return new FormGroup({
        layout: new FormGroup({
          enabled: new FormControl<boolean>(false),
          position: new FormControl<Position>('top'),
        }),
      });
    }
    default: {
      return new FormGroup({});
    }
  }
};
