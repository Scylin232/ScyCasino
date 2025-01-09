import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthConfig, OAuthEvent, OAuthService } from 'angular-oauth2-oidc';

import { ClaimModel } from '../../shared/models/claim.model';

import { environment } from '../../../environments/environment';
import {User} from '../../shared/models/user.model';
import {Observable} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly oauthService: OAuthService = inject(OAuthService);
  private readonly http: HttpClient = inject(HttpClient);

  private readonly authConfig: AuthConfig = {
    issuer: environment.authIssuer,
    redirectUri: environment.authRedirectUri,
    clientId: "scycasino-auth-client-id",
    scope: "openid offline_access resource",
    responseType: "code",
    strictDiscoveryDocumentValidation: false,
  };

  constructor() {
    this.configureAuth();
  }

  public login(): void {
    this.oauthService.initCodeFlow();
  }

  public logout(): void {
    this.oauthService.logOut();
  }

  public get isLoggedIn(): boolean {
    return this.oauthService.hasValidAccessToken();
  }

  public get claims(): ClaimModel {
    return this.oauthService.getIdentityClaims() as ClaimModel;
  }

  public get accessToken(): string {
    return this.oauthService.getAccessToken();
  }

  public get user(): Observable<User> {
    return this.http.get<User>(`${environment.apiUrl}/api/user/self`);
  }

  private configureAuth(): void {
    this.oauthService.configure(this.authConfig);

    this.oauthService.loadDiscoveryDocumentAndTryLogin().then((): void => {
      if (this.oauthService.hasValidAccessToken())
        this.notifyOAuthComplete();
    });

    this.oauthService.events.subscribe((event: OAuthEvent): void => {
      if (event.type == "token_received")
        this.notifyOAuthComplete();
    });

    this.oauthService.setupAutomaticSilentRefresh();
  }

  private notifyOAuthComplete(): void {
    if (!this.claims || !this.oauthService.hasValidAccessToken()) return;

    this.http.post(`${environment.apiUrl}/user/oauth-complete`, null).subscribe();
  }
}
