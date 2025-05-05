import { bootstrapApplication } from '@angular/platform-browser';
import { APP_CONFIG } from './app.config';
import { WorkspaceComponent } from '../pages';

bootstrapApplication(WorkspaceComponent, APP_CONFIG).catch((err) =>
  console.error(err),
);
