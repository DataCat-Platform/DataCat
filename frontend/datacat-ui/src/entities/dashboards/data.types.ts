export type DataPoint = {
  value: number;
  timestamp: string;
};

export type TimeSeries = {
  metric?: string;
  labels?: { [key: string]: string; };
  dataPoints: DataPoint[];
}
