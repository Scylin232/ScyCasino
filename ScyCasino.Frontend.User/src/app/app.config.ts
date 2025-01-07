import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { provideOAuthClient, OAuthStorage } from 'angular-oauth2-oidc';

import { routes } from './app.routes';
import { authInterceptor } from './core/auth/auth-interceptor';

import { environment } from '../environments/environment';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(
      withInterceptors([authInterceptor])
    ),
    provideOAuthClient({
      resourceServer: {
        allowedUrls: [environment.apiUrl, "http://localhost:9231"],
        sendAccessToken: true,
      }
    }),
    {
      provide: OAuthStorage,
      useFactory: (): OAuthStorage => localStorage,
    }
  ]
};
