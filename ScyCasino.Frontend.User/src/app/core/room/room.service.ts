import {Observable, Subscriber} from 'rxjs';
import {inject, Injectable} from '@angular/core';
import {HttpTransportType, HubConnection, HubConnectionBuilder} from '@microsoft/signalr'

import {AuthService} from '../auth/auth.service';

import {environment} from '../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class RoomService {
  private hubConnection!: HubConnection;
  private authService: AuthService = inject(AuthService);

  public initializeConnection(roomId: string): void {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(`${environment.apiUrl}/hub/room?roomId=${encodeURIComponent(roomId)}`, {
        skipNegotiation: true,
        transport: HttpTransportType.WebSockets,
        accessTokenFactory: (): string => this.authService.accessToken
      })
      .build();
  }

  public startConnection(): Observable<void> {
    return new Observable<void>((observer: Subscriber<void>): void => {
      this.hubConnection
        .start()
        .then((): void => {
          observer.next();
          observer.complete();
        })
        .catch((error: any): void => {
          observer.error(error);
        });
    });
  }

  public stopConnection(): Observable<void> {
    return new Observable<void>((observer: Subscriber<void>): void => {
      this.hubConnection
        .stop()
        .then((): void => {
          observer.next();
          observer.complete();
        })
        .catch((error: any): void => {
          observer.error(error);
        })
    });
  }

  public eventReceived(eventName: string): Observable<string> {
    return new Observable<string>((observer: Subscriber<string>): void => {
      this.hubConnection.on(eventName, (message: string): void => {
        observer.next(message);
      });
    });
  }

  public sendEvent(eventName: string, ...args: any[]): void {
    this.hubConnection.invoke(eventName, ...args);
  }
}
