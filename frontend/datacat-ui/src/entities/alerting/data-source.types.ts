export enum DataSourceDriver {
  PROMETHEUS = 'prometheus'
};

export type DataSource = {
  id: string;
  name: string;
  driver: DataSourceDriver;

  // TODO: might be better to replace with abstract 'settings'
  // that depend on the driver.
  connectionUrl: string;
};