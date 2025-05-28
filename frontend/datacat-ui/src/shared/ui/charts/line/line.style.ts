import { z } from 'zod/v4';

export const LineStyleScheme = z.object({
  title: z.object({
    enabled: z.boolean().default(false),
    text: z.string().default(''),
  }),
  legend: z.object({
    enabled: z.boolean().default(true),
    position: z.enum(['top', 'left', 'bottom', 'right']).default('top'),
  }),
  tooltip: z.object({
    enabled: z.boolean().default(true),
  }),
  axis: z.object({
    xAxisTitle: z.string().default('Date').optional(),
    xAxisMin: z.date().optional(),
    xAxisMax: z.date().optional(),
    yAxisTitle: z.string().default('Value').optional(),
    yAxisMin: z.number().optional(),
    yAxisMax: z.number().optional(),
  }),
});

export type LineStyle = z.infer<typeof LineStyleScheme>;
