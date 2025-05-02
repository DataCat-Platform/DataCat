import { AlertStatus } from '../../../entities';
import { AlertsCountsByStatus, AlertsFilter } from './alerts-explorer.types';

export const DEFAULT_ALERTS_FILTER: AlertsFilter = {
  tags: [],
  page: 0,
  pageSize: 5,
};

export const getDefaultAlertsCountsByStatus = (): AlertsCountsByStatus => {
  const counts = new Map<AlertStatus, number>();
  Object.values(AlertStatus).forEach((status) => counts.set(status, 0));
  return counts;
};
