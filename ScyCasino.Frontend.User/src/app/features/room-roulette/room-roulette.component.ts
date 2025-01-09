import {Component, inject, OnDestroy, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {HttpClient} from '@angular/common/http';

import {RoomService} from '../../core/room/room.service';
import {RouletteBetTableComponent} from '../../shared/components/roulette/roulette-bet-table/roulette-bet-table.component';
import {RouletteBetListComponent} from '../../shared/components/roulette/roulette-bet-list/roulette-bet-list.component';
import {PlayerListComponent} from '../../shared/components/player-list/player-list.component';
import {RouletteBet} from '../../shared/models/roulette.model';

import {environment} from '../../../environments/environment';

@Component({
  selector: 'app-room-roulette',
  imports: [
    RouletteBetTableComponent,
    RouletteBetListComponent,
    PlayerListComponent,
  ],
  templateUrl: './room-roulette.component.html',
  styleUrl: './room-roulette.component.css'
})
export class RoomRouletteComponent implements OnInit, OnDestroy {
  private readonly route: ActivatedRoute = inject(ActivatedRoute);
  private readonly http: HttpClient = inject(HttpClient);
  private readonly roomService: RoomService = inject(RoomService);

  public rouletteBets: RouletteBet[] = [];
  public players: string[] = [];

  ngOnInit(): void {
    const roomId: string | null = this.route.snapshot.paramMap.get('roomId');

    if (!roomId) return;

    this.roomService.initializeConnection(roomId);

    this.roomService
      .startConnection()
      .subscribe((): void => {
        this.roomService.eventReceived("PlayerListUpdated").subscribe((updatedPlayerList: string): void => {
          this.players = updatedPlayerList as unknown as string[];
        });

        this.roomService.eventReceived("GameStateUpdated").subscribe((updatedGameState: string): void => {
          this.rouletteBets = JSON.parse(updatedGameState) as RouletteBet[];
        });

        this.roomService.eventReceived("RoundEnded").subscribe((roundEndInfo: string): void => {
          this.rouletteBets = [];
        });
      });

    this.http.get<RouletteBet[]>(`${environment.apiUrl}/api/roulette/current-bets?roomId=${roomId}`).subscribe({
      next: (data: RouletteBet[]): void => {
        this.rouletteBets = data;
      }
    });
  }

  ngOnDestroy(): void {
    this.roomService
      .stopConnection()
      .subscribe();
  }
}
