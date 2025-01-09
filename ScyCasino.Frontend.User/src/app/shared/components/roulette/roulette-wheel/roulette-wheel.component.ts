import {Component, Input, OnInit} from '@angular/core';
import {NgForOf} from '@angular/common';
import {RouletteBetType, rouletteNumbers} from '../../../models/roulette.model';
import {getRouletteBetButtonColor} from '../../../utils/roulette.utils';
import {Subject} from 'rxjs';

@Component({
  selector: 'app-roulette-wheel',
  imports: [
    NgForOf
  ],
  templateUrl: './roulette-wheel.component.html',
  styleUrl: './roulette-wheel.component.css'
})
export class RouletteWheelComponent implements OnInit {
  @Input() winningNumber!: Subject<number>;

  protected readonly sortedRouletteNumbers = [0, ...rouletteNumbers.sort((a, b) => a - b)];
  protected readonly getRouletteBetButtonColor = getRouletteBetButtonColor;
  protected readonly RouletteBetType = RouletteBetType;

  public wheelTransition: string = "";

  ngOnInit(): void {
    this.winningNumber.subscribe((winningNumber: number) => {
      this.wheelTransition = `-${winningNumber * 64}px`;
      console.log(winningNumber);
    })
  }
}
