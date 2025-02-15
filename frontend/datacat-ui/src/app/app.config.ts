import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideAnimationsAsync } from "@angular/platform-browser/animations/async";
import { providePrimeNG } from "primeng/config";
import { DataCatTheme } from "./presets/datacat-theme";
import { provideHttpClient, withInterceptors } from "@angular/common/http";
import { apiInterceptor } from "./core/interceptors/api.interceptor";


export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({eventCoalescing: true}),
    provideRouter(routes),
    provideAnimationsAsync(),
    provideHttpClient(
      withInterceptors([apiInterceptor]),
    ),
    providePrimeNG({
      theme: {
        preset: DataCatTheme,
      }
    })
  ]
};

