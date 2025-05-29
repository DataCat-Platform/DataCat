import { z } from 'zod/v4';

export const LineStyleScheme = z.object({
  title: z
    .object({
      enabled: z.boolean().default(false),
      text: z.string().default(''),
    })
    .default({
      enabled: false,
      text: '',
    }),
  legend: z
    .object({
      enabled: z.boolean().default(true),
      position: z.enum(['top', 'left', 'bottom', 'right']).default('top'),
    })
    .default({
      enabled: true,
      position: 'top',
    }),
  tooltip: z
    .object({
      enabled: z.boolean().default(true),
    })
    .default({
      enabled: true,
    }),
  axis: z
    .object({
      xAxisTitle: z.string().default('Date').nullish(),
      xAxisMin: z.date().nullish(),
      xAxisMax: z.date().nullish(),
      yAxisTitle: z.string().default('Value').nullish(),
      yAxisMin: z.number().nullish(),
      yAxisMax: z.number().nullish(),
    })
    .default({}),
});

export type LineStyle = z.infer<typeof LineStyleScheme>;
