export type DataPoint = {
  value: number;
  timestamp: Date;
};

export type TimeSeries = {
  metric?: string;
  labels?: { [key: string]: string };
  dataPoints: DataPoint[];
};
