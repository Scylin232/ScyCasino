import {Component, inject, Input, OnChanges, SimpleChanges} from '@angular/core';
import {RouletteBet, RouletteBetType} from '../../../models/roulette.model';
import {NgForOf} from '@angular/common';
import {getColorFromUserId} from '../../../utils/user.utils';
import {AuthService} from '../../../../core/auth/auth.service';
import {User} from '../../../models/user.model';

@Component({
  selector: 'app-roulette-bet-list',
  imports: [
    NgForOf
  ],
  templateUrl: './roulette-bet-list.component.html',
  styleUrl: './roulette-bet-list.component.css'
})
export class RouletteBetListComponent implements OnChanges {
  @Input() rouletteBets: RouletteBet[] = [];
  @Input() coins: number = 0;

  protected readonly RouletteBetType = RouletteBetType;
  protected readonly getColorFromUserId = getColorFromUserId;

  private readonly authService: AuthService = inject(AuthService);

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['rouletteBets'].firstChange) return;

    this.authService.user.subscribe({
      next: (user: User): void => {
        this.coins = user.coins;
      }
    })
  }
}
