import { z } from 'zod/v4';

export const PieStyleScheme = z.object({
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
});

export type PieStyle = z.infer<typeof PieStyleScheme>;
