import { DataSource } from '../alerting';

export type PanelType = {
  id: number;
  type: VisualizationType;
};

export enum VisualizationType {
  LINE = 'line',
  BAR = 'bar',
  PIE = 'pie',
  UNKNOWN = 'unknown',
}

export type Position = 'top' | 'bottom' | 'left' | 'right';

export interface BasicVisaulizationOptions {
  legend?: {
    enabled?: boolean;
    position?: Position;
  };
  title?: {
    enabled?: boolean;
    text?: string;
  };
  tooltip?: {
    enabled?: boolean;
  };
}

export interface LineVisualizationOptions extends BasicVisaulizationOptions {}
export interface BarVisualizationOptions extends BasicVisaulizationOptions {}
export interface PieVisualizationOptions extends BasicVisaulizationOptions {}

export type VisualizationSettings =
  | LineVisualizationOptions
  | BarVisualizationOptions
  | PieVisualizationOptions;

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
  visualizationSettings?: VisualizationSettings;
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

export const decodeVisualizationSettings = (
  settings: string | undefined,
): VisualizationSettings => {
  if (!settings) {
    return {};
  }
  try {
    return JSON.parse(settings);
  } catch {
    return {};
  }
};

export const encodeVisualizationSettings = (
  settings: VisualizationSettings | undefined,
): string => {
  return JSON.stringify(settings);
};

export const encodeVisualizationType = (
  type: VisualizationType | undefined,
): number => {
  switch (type) {
    case VisualizationType.LINE:
      return 1;
    case VisualizationType.BAR:
      return 3;
    case VisualizationType.PIE:
      return 2;
    default:
      return 4;
  }
};

export const decodeVisualizationType = (
  type: string | undefined,
): VisualizationType => {
  switch (type) {
    case 'LineChart':
      return VisualizationType.LINE;
    case 'BarChart':
      return VisualizationType.BAR;
    case 'PieChart':
      return VisualizationType.PIE;
    default:
      return VisualizationType.UNKNOWN;
  }
};
