import { Component } from '@angular/core';
import {NgForOf} from "@angular/common";
import {RouletteBetButtonComponent} from "../roulette-bet-button/roulette-bet-button.component";

import { rouletteNumbers, blackRouletteNumbers, redRouletteNumbers, RouletteBetType } from '../../../models/roulette.model';
import { generateSequenceWithStep, generateEvenSequence, generateOddSequence } from '../../../utils/sequence.utils';

@Component({
  selector: 'app-roulette-bet-table',
  imports: [
    NgForOf,
    RouletteBetButtonComponent
  ],
  templateUrl: './roulette-bet-table.component.html',
  styleUrl: './roulette-bet-table.component.css'
})
export class RouletteBetTableComponent {
  protected readonly rouletteNumbers = rouletteNumbers;
  protected readonly redRouletteNumbers = redRouletteNumbers;
  protected readonly blackRouletteNumbers = blackRouletteNumbers;
  protected readonly RouletteBetType = RouletteBetType;
  protected readonly generateSequenceWithStep = generateSequenceWithStep;
  protected readonly generateEvenSequence = generateEvenSequence;
  protected readonly generateOddSequence = generateOddSequence;
}
