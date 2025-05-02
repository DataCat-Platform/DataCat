export enum DataSourceType {
  PROMETHEUS = 'prometheus',
}

export interface DataSource {
  id: string;
  name: string;
  type: DataSourceType;
  connectionUrl: string;
}
