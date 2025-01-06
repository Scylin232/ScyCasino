import {Component, inject, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';

import {RoomService} from '../../core/room/room.service';

@Component({
  selector: 'app-room-roulette',
  imports: [],
  templateUrl: './room-roulette.component.html',
  styleUrl: './room-roulette.component.css'
})
export class RoomRouletteComponent implements OnInit {
  private readonly route: ActivatedRoute = inject(ActivatedRoute);
  private readonly gameService: RoomService = inject(RoomService);

  ngOnInit(): void {
    const roomId: string | null = this.route.snapshot.paramMap.get('roomId');

    console.log(roomId);

    this.gameService
      .startConnection()
      .subscribe((): void => {
        this.gameService.eventReceived("PlayerListUpdated").subscribe((updatedPlayerList: string): void => {
          console.log("Updated Player List: ", updatedPlayerList);
        });

        this.gameService.sendEvent("JoinRoom", roomId);
      });
  }
}
