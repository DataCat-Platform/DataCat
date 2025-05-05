export enum VisualizationType {
  LINE = 'line',
  BAR = 'bar',
  PIE = 'pie',
  GAUGE = 'gauge',
  TABLE = 'table',
}

export type Visualization = {
  type: VisualizationType;
  settings: string;
};

export type Panel = {
  id: string;
  title: string;
  rawQuery: string;
  dashboardId: string;
  visualization: Visualization;
};
