import {Component, inject} from '@angular/core';
import {Router, RouterLink} from '@angular/router';
import {FormControl, FormGroup, ReactiveFormsModule} from '@angular/forms';
import {HttpClient, HttpErrorResponse} from '@angular/common/http';

import {RoomType} from '../../shared/models/room.model';

import {environment} from '../../../environments/environment';

@Component({
  selector: 'app-room-create',
  imports: [
    RouterLink,
    ReactiveFormsModule
  ],
  templateUrl: './room-create.component.html',
  styleUrl: './room-create.component.css'
})
export class RoomCreateComponent {
  protected readonly RoomType: typeof RoomType = RoomType;
  private readonly http: HttpClient = inject(HttpClient);
  private readonly router: Router = inject(Router);

  roomCreateForm = new FormGroup({
    roomName: new FormControl(''),
    roomType: new FormControl(0)
  });

  public createRoom(): void {
    const roomName: string | null | undefined = this.roomCreateForm.value.roomName;
    const roomType: number | null | undefined = this.roomCreateForm.value.roomType;

    if (!roomName || roomType === null || roomType === undefined) return;

    this.http.post(`${environment.apiUrl}/api/room`, {
      name: roomName,
      roomType
    }).subscribe({
      next: (response: any): void => {
        this.router.navigate(["/rooms"]);
      }
    });
  }
}
