export type TimeSeries = {
  name: string;
  labels: { [key: string]: string };
  points: DataPoint[];
};

export type DataPoint = {
  value: number;
  timestamp: Date;
  labels: { [key: string]: string };
};
