import { Observable } from 'rxjs';
import { inject } from '@angular/core';
import { HttpEvent, HttpHandlerFn, HttpHeaders, HttpInterceptorFn, HttpRequest } from '@angular/common/http';
import { OAuthStorage, OAuthModuleConfig } from 'angular-oauth2-oidc';

export const authInterceptor: HttpInterceptorFn = (req: HttpRequest<unknown>, next: HttpHandlerFn): Observable<HttpEvent<unknown>> => {
  const oauthStorage: OAuthStorage = inject(OAuthStorage);
  const oauthModuleConfig: OAuthModuleConfig = inject(OAuthModuleConfig);

  let url: string = req.url.toLowerCase();

  if (!oauthModuleConfig || !oauthModuleConfig.resourceServer || !oauthModuleConfig.resourceServer.allowedUrls || !checkUrl(url, oauthModuleConfig))
    return next(req);

  const sendAccessToken: boolean = oauthModuleConfig.resourceServer.sendAccessToken;

  if (!sendAccessToken)
    return next(req);

  const token: string | null = oauthStorage.getItem("access_token");
  const headers: HttpHeaders = req.headers.set("Authorization", `Bearer ${token}`);

  const clonedRequest: HttpRequest<unknown> = req.clone({ headers });

  return next(clonedRequest);
};

function checkUrl(url: string, oauthModuleConfig: OAuthModuleConfig): boolean {
  const found: string | undefined = oauthModuleConfig.resourceServer.allowedUrls?.find((u: string): boolean => url.startsWith(u));
  return !!found;
}
