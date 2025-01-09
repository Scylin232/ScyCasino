import {Component, inject, Input} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {ActivatedRoute} from '@angular/router';

import {blackRouletteNumbers, redRouletteNumbers, RouletteBetType} from '../../../models/roulette.model';
import {environment} from '../../../../../environments/environment';

@Component({
  selector: 'app-roulette-bet-button',
  imports: [],
  templateUrl: './roulette-bet-button.component.html',
  styleUrl: './roulette-bet-button.component.css'
})
export class RouletteBetButtonComponent {
  @Input() betType!: RouletteBetType;
  @Input() betValue!: number[];

  private readonly route: ActivatedRoute = inject(ActivatedRoute);
  private readonly http: HttpClient = inject(HttpClient);

  public getRouletteBetButtonColor(): string {
    if (this.betType == RouletteBetType.Black || (this.betType == RouletteBetType.Straight && blackRouletteNumbers.includes(this.betValue[0])))
      return "#41444B";

    if (this.betType == RouletteBetType.Red || (this.betType == RouletteBetType.Straight && redRouletteNumbers.includes(this.betValue[0])))
      return "#FF7457";

    return "#95c361";
  }

  public getRouletteBetButtonLabel(): string {
    switch (this.betType)
    {
      case RouletteBetType.Straight:
        return this.betValue[0].toString();
      case RouletteBetType.Column:
        return "2to1";
      case RouletteBetType.Range:
        return this.betValue[0] == 1 ? "1to18" : "19to36";
      case RouletteBetType.Dozen:
        return this.betValue[0] == 1 ? "1st12" : this.betValue[0] == 13 ? "2nd12" : "3rd12";
      default:
        return RouletteBetType[this.betType];
    }
  }

  public placeBet(): void {
    const roomId: string | null = this.route.snapshot.paramMap.get('roomId');
    this.http.post(`${environment.apiUrl}/api/roulette/bet?roomId=${roomId}&amount=${10}&betType=${this.betType}`, this.betValue).subscribe()
  }
}
