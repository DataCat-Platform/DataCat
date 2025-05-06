import {ApplicationConfig, provideZoneChangeDetection} from '@angular/core';
import {provideRouter, withComponentInputBinding} from '@angular/router';
import {ROUTES} from '../pages/workspace.routes';
import {provideAnimationsAsync} from '@angular/platform-browser/animations/async';
import {providePrimeNG} from 'primeng/config';
import {PRIMENG_CONFIG} from '../shared/primeng/primeng.config';
import {provideHttpClient, withInterceptors} from '@angular/common/http';
import {apiInterceptor} from '../shared/interceptors/api.interceptor';
import {authInterceptor} from "../shared/interceptors/auth.interceptor";

export const APP_CONFIG: ApplicationConfig = {
    providers: [
        provideZoneChangeDetection({eventCoalescing: true}),
        provideRouter(ROUTES, withComponentInputBinding()),
        provideAnimationsAsync(),
        provideHttpClient(withInterceptors([
            apiInterceptor,
            authInterceptor
        ])),
        providePrimeNG(PRIMENG_CONFIG),
    ],
};
