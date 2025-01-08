import {Component, inject, OnDestroy, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';

import {RoomService} from '../../core/room/room.service';
import {HttpClient} from '@angular/common/http';

@Component({
  selector: 'app-room-roulette',
  imports: [],
  templateUrl: './room-roulette.component.html',
  styleUrl: './room-roulette.component.css'
})
export class RoomRouletteComponent implements OnInit, OnDestroy {
  private readonly route: ActivatedRoute = inject(ActivatedRoute);
  private readonly http: HttpClient = inject(HttpClient);
  private readonly roomService: RoomService = inject(RoomService);

  ngOnInit(): void {
    const roomId: string | null = this.route.snapshot.paramMap.get('roomId');

    if (!roomId) return;

    this.roomService.initializeConnection(roomId);

    this.roomService
      .startConnection()
      .subscribe((): void => {
        this.roomService.eventReceived("PlayerListUpdated").subscribe((updatedPlayerList: string): void => {
          console.log("Updated Player List: ", updatedPlayerList);
        });

        this.roomService.eventReceived("GameStateUpdated").subscribe((updatedGameState: string): void => {
          console.log("Updated Game State: ", JSON.parse(updatedGameState));
        });
        
        this.roomService.eventReceived("RoundEnded").subscribe((roundEndInfo: string): void => {
          console.log(roundEndInfo);
        });
      });
  }

  ngOnDestroy(): void {
    this.roomService
      .stopConnection()
      .subscribe();
  }

  public placeBet(): void {
    const roomId: string | null = this.route.snapshot.paramMap.get('roomId');

    this.http.post(`http://localhost:9231/api/Roulette/bet?roomId=${roomId}&amount=10&betType=0`, [0]).subscribe()
  }
}
