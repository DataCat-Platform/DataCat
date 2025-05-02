export type DashboardInSearchList = {
  id: string;
  name: string;
  description?: string;
  ownerId: string;
  lastUpdatedAt: number;
  tags: string[];
  isInFavorites: boolean;
};

export type Dashboard = {
  id: string;
  name: string;
  description?: string;
  ownerId: string;
  lastUpdatedAt: number;
  panels: Panel[];
  tags: string[];
  isInFavorites: boolean;
};

export type DashboardsFilter = {
  onlyFavorites?: boolean;
  tags: string[];
};

export type Panel = {
  id: string;
  title: string;
  type: PanelType;
  rawQuery: string;
  dashboardId: string;
  layout: PanelLayout;
};

export type PanelLayout = {
  x: number;
  y: number;
  width: number;
  height: number;
};

export enum PanelType {
  LINE = 'line',
  BAR = 'bar',
  PIE = 'pie',
  GAUGE = 'gauge',
  TABLE = 'table',
}
