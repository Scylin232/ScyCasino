import { Routes } from '@angular/router';

import {LeaderboardComponent} from './features/leaderboard/leaderboard.component';
import {RoomListComponent} from './features/room-list/room-list.component';
import {RoomCreateComponent} from './features/room-create/room-create.component';
import {RoomRouletteComponent} from './features/room-roulette/room-roulette.component';
import {HomeComponent} from './features/home/home.component';
import {authGuard} from './core/auth/auth.guard';

export const routes: Routes = [
  {
    path: "",
    component: HomeComponent
  },
  {
    path: "leaderboard",
    component: LeaderboardComponent
  },
  {
    path: "rooms",
    component: RoomListComponent
  },
  {
    path: "rooms/create",
    component: RoomCreateComponent,
    canActivate: [authGuard]
  },
  {
    path: "rooms/0/:roomId",
    component: RoomRouletteComponent,
    canActivate: [authGuard]
  }
];
