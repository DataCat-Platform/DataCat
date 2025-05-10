export const BASIC_OPTIONS = {
  animation: false,
  maintainAspectRatio: false,
  scales: {
    x: {
      min: 0,
      max: 10,
      ticks: {},
    },
    y: {
      min: 0,
      max: 10,
      ticks: {},
    },
  },
  plugins: {
    legend: {
      display: false,
      position: 'top',
    },
    title: {
      display: true,
      text: 'asf',
    },
    tooltip: {
      enabled: true,
    },
  },
  parsing: {
    key: 'x',
    xAxisKey: 'x',
    yAxisKey: 'y',
  },
  layout: {
    padding: null,
  },
};
