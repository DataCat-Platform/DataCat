import { AlertStatus } from "./alert.types";

export type AlertsFilter = {
  status?: AlertStatus;
  tags: string[],
  dataSourceId?: string;
};