import { FormControl, FormGroup } from '@angular/forms';
import { VisualizationType } from '../../../entities';

const createForm = (type: VisualizationType) => {
  switch (type) {
    case VisualizationType.LINE: {
      return new FormGroup({});
    }
    default: {
      return new FormGroup({});
    }
  }
};
