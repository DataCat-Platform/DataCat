export type TimeSeries = {
  name: string;
  labels: string[];
  points: DataPoint[];
};

export type DataPoint = {
  value: number;
  timestamp: Date;
  labels: string[];
};
