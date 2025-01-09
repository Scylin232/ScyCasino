import {inject, Component, OnInit} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {NgForOf} from '@angular/common';
import {RouterLink} from '@angular/router';

import {Room} from '../../shared/models/room.model';
import {PaginatedResponse} from '../../shared/models/api.model';

import {environment} from '../../../environments/environment';

@Component({
  selector: 'app-room-list',
  imports: [
    NgForOf,
    RouterLink
  ],
  templateUrl: './room-list.component.html',
  styleUrl: './room-list.component.css'
})
export class RoomListComponent implements OnInit {
  private readonly http: HttpClient = inject(HttpClient);

  public rooms: Room[] = [];

  public ngOnInit(): void {
    this.fetchRooms();
  }

  public fetchRooms(): void {
    this.http.get<PaginatedResponse<Room>>(`${environment.apiUrl}/api/room/all?page=1&count=15`).subscribe({
      next: (data: PaginatedResponse<Room>): void => {
        this.rooms = data.results;
      },
    })
  }

  protected readonly Object = Object;
}
