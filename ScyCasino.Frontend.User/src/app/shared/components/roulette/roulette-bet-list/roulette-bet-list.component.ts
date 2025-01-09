import {Component, Input} from '@angular/core';
import {RouletteBet, RouletteBetType} from '../../../models/roulette.model';
import {NgForOf} from '@angular/common';
import {getColorFromUserId} from '../../../utils/user.utils';

@Component({
  selector: 'app-roulette-bet-list',
  imports: [
    NgForOf
  ],
  templateUrl: './roulette-bet-list.component.html',
  styleUrl: './roulette-bet-list.component.css'
})
export class RouletteBetListComponent {
  @Input() rouletteBets: RouletteBet[] = [];

  protected readonly RouletteBetType = RouletteBetType;
  protected readonly getColorFromUserId = getColorFromUserId;
}
