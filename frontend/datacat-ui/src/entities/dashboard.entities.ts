export interface DashboardInSearchList {
  id: string;
  name: string;
  description?: string;
  ownerId: string;
  lastUpdatedAt: Date;
}

export interface Dashboard {
  id: string;
  name: string;
  description?: string;
  ownerId: string;
  lastUpdatedAt: Date;
  panels: Panel[];
}

export enum PanelType {
  LINE = 'line',
  BAR = 'bar',
}

export interface Panel {
  id: string;
  title: string;
  type: PanelType;
  rawQuery: string;
  dashboardId: string;
  layout: PanelLayout;
}

export interface PanelLayout {
  x: number;
  y: number;
  width: number;
  height: number;
}
