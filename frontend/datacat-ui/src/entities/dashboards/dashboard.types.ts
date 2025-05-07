import { Panel } from './panel.types';

export type Dashboard = {
  id: string;
  name: string;
  description: string;
  panels: Panel[];
  ownerId: string;
  lastUpdatedAt: number;
};
