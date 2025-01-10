import {Component, Input, OnDestroy, OnInit} from '@angular/core';
import {Subject} from 'rxjs';
import {NgForOf} from '@angular/common';

import {RouletteBetType, rouletteNumbers} from '../../../models/roulette.model';
import {getRouletteBetButtonColor} from '../../../utils/roulette.utils';

@Component({
  selector: 'app-roulette-wheel',
  imports: [
    NgForOf
  ],
  templateUrl: './roulette-wheel.component.html',
  styleUrl: './roulette-wheel.component.css'
})
export class RouletteWheelComponent implements OnInit, OnDestroy {
  @Input() winningNumber!: Subject<number>;

  protected readonly sortedRouletteNumbers = [0, ...rouletteNumbers.sort((a, b) => a - b)];
  protected readonly getRouletteBetButtonColor = getRouletteBetButtonColor;
  protected readonly RouletteBetType = RouletteBetType;

  public wheelTransition: string = "";
  public timerLabel: string = "";
  private timerId: any = null;

  ngOnInit(): void {
    this.winningNumber.subscribe((winningNumber: number) => {
      this.wheelTransition = `-${winningNumber * 64}px`;
    });

    this.updateRemainingTime();
    this.timerId = setInterval((): void => {
      this.updateRemainingTime();
    }, 1000);
  }

  ngOnDestroy(): void {
    if (!this.timerId) return;

    clearInterval(this.timerId);
  }

  updateRemainingTime(): void {
    const now = new Date();
    const secondsToNextMinute: number = 60 - now.getSeconds();

    this.timerLabel = `${secondsToNextMinute} seconds`;
  }
}
