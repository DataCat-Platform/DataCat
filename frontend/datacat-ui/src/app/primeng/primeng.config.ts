import { PrimeNGConfigType } from 'primeng/config';
import { DATACAT_PRIMENG_PRESET } from './primeng.preset';

export const PRIMENG_CONFIG: PrimeNGConfigType = {
  theme: {
    preset: DATACAT_PRIMENG_PRESET,
    options: {
      darkModeSelector: '.use-dark-theme',
    },
  },
};
