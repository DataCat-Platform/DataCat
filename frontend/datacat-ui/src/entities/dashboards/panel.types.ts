import { DataSource } from '../alerting';

export enum VisualizationType {
  LINE = 'line',
  BAR = 'bar',
  PIE = 'pie',
  GAUGE = 'gauge',
  TABLE = 'table',
  UNKNOWN = 'unknown',
}

export type VisualizationSettings = {};

export type Layout = {
  x: number;
  y: number;
  rows: number;
  cols: number;
};

export type Panel = {
  id: string;
  title: string;
  query: string;
  dataSource?: DataSource;
  layout: Layout;
  visualizationType?: VisualizationType;
  visualizationSetttings?: VisualizationSettings;
};

export type LineStyle = {
  lineWidth: number;
};

export const decodeLayout = (encoded: string | undefined): Layout => {
  if (encoded) {
    try {
      const layout = JSON.parse(encoded);
      return layout;
    } catch {}
  }

  return {
    x: 0,
    y: 0,
    cols: 5,
    rows: 3,
  };
};

export const encodeLayout = (layout: Layout): string => {
  return JSON.stringify(layout);
};
