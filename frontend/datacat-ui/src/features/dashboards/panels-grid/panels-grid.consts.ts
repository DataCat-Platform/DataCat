import { TimeRange } from '../../../entities/dashboards/etc.types';
import { RefreshRateOption } from './panels-grid.types';

export const REFRESH_RATE_OPTIONS: RefreshRateOption[] = [
  { title: 'off', seconds: null },
  { title: '10s', seconds: 10 },
  { title: '30s', seconds: 30 },
  { title: '1m', seconds: 60 },
  { title: '10m', seconds: 600 },
  { title: '1h', seconds: 3600 },
];

export const DEFAULT_TIME_RANGE: TimeRange = {
  step: '00:30:00',
  from: (() => {
    const date = new Date();
    date.setMinutes(date.getMinutes() - 360);
    return date;
  })(),
  to: new Date(),
};
