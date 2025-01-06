import { Routes } from '@angular/router';

import {LeaderboardComponent} from './features/leaderboard/leaderboard.component';
import {RoomListComponent} from './features/room-list/room-list.component';
import {RoomRouletteComponent} from './features/room-roulette/room-roulette.component';

export const routes: Routes = [
  {
    path: "leaderboard",
    component: LeaderboardComponent
  },
  {
    path: "rooms",
    component: RoomListComponent
  },
  {
    path: "rooms/0/:roomId",
    component: RoomRouletteComponent
  }
];
