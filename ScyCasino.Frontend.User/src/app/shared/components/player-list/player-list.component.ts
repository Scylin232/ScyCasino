import {Component, inject, Input, OnChanges, OnDestroy, OnInit} from '@angular/core';
import {NgForOf} from '@angular/common';
import {MediaConnection, Peer} from 'peerjs';
import {AuthService} from '../../../core/auth/auth.service';
import {User} from '../../models/user.model';

@Component({
  selector: 'app-player-list',
  imports: [
    NgForOf
  ],
  templateUrl: './player-list.component.html',
  styleUrl: './player-list.component.css'
})
export class PlayerListComponent implements OnInit, OnChanges, OnDestroy {
  @Input() players: string[] = [];

  protected readonly authService: AuthService = inject(AuthService);
  protected readonly Object: ObjectConstructor = Object;

  private peer: Peer | undefined;
  private localStream: MediaStream | undefined;

  public playerStreams: { [key: string]: MediaStream } = {};

  ngOnInit(): void {
    this.authService.user.subscribe({
      next: (user: User): void => {
        const userId: string = user.id;
        this.peer = new Peer(userId);

        navigator.mediaDevices.getUserMedia({
          video: true,
          audio: true
        }).then((stream: MediaStream): void => {
          this.localStream = stream;
          this.playerStreams[userId] = stream;
        });

        this.peer.on('call', (call: MediaConnection): void => {
          call.answer(this.localStream);
          call.on('stream', (remoteStream: MediaStream): void => {
            this.playerStreams[call.peer] = remoteStream;
          });
          call.on('close', (): void => {
            delete this.playerStreams[call.peer];
          });
        });
      }
    });
  }

  ngOnChanges(): void {
    if (this.players.length <= 0) return;

    // TODO: Publish an event about peer in SignalR and use data from it.
    setTimeout((): void => {
      for (const player of this.players) {
        if (this.peer!.id == player) continue;
        this.peer!.call(player, this.localStream!);
      }
    }, 5000);
  }

  ngOnDestroy(): void {
    if (this.peer)
      this.peer.destroy();

    if (this.localStream)
      this.localStream.getTracks().forEach((track: MediaStreamTrack): void => track.stop());
  }
}
