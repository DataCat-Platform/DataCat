import {
  DataSourceResponse,
  GetPanelResponse,
  VariableResponse,
} from '../../shared/services/datacat-generated-client';
import { DataSource } from '../alerting';
import {
  Layout,
  Panel,
  VisualizationSettings,
  VisualizationType,
} from './panel.types';
import { DashboardVariable } from './variables.types';

const parseLayout = (s: string): Layout => {
  try {
    return JSON.parse(s);
  } catch {
    return {
      x: 0,
      y: 0,
      rows: 3,
      cols: 5,
    };
  }
};

const parseVisualizationSettings = (s: string): VisualizationSettings => {
  try {
    return JSON.parse(s);
  } catch {
    return {};
  }
};

const parseVisualizationType = (s: string): VisualizationType => {
  switch (s) {
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

export const mapDataSourceResponseToDataSource = (
  r: DataSourceResponse,
): DataSource => {
  return {
    id: r.id!,
    name: r.name!,
    driver: r.type! as any,
    connectionUrl: r.connectionString!,
  };
};

export const mapGetPanelResponeToPanel = (r: GetPanelResponse): Panel => {
  return {
    id: r.id!,
    title: r.title!,
    query: r.query!.query!,
    dataSource: mapDataSourceResponseToDataSource(r.query!.dataSource!),
    layout: parseLayout(r.layout!),
    visualizationType: parseVisualizationType(r.typeName!),
    visualizationSettings: parseVisualizationSettings(r.styleConfiguration!),
  };
};

export const mapVariableResponseToDashboardVariable = (
  r: VariableResponse,
): DashboardVariable => {
  return {
    id: r.id!,
    placeholder: r.placeholder!,
    value: r.value!,
  };
};

export const serializeVisualizationSettings = (
  vs: VisualizationSettings,
): string => {
  return JSON.stringify(vs);
};

export const serializeLayout = (layout: Layout): string => {
  return JSON.stringify(layout);
};

export const serializeVisualizationType = (type: VisualizationType): number => {
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
