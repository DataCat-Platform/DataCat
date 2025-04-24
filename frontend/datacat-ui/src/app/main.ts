import { bootstrapApplication } from '@angular/platform-browser';
import { APP_CONFIG } from './app.config';
import { MainPageComponent } from '../pages';

bootstrapApplication(MainPageComponent, APP_CONFIG).catch((err) =>
  console.error(err),
);
