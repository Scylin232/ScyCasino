import {Component, inject, Input} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {ActivatedRoute} from '@angular/router';

import {blackRouletteNumbers, redRouletteNumbers, RouletteBetType} from '../../../models/roulette.model';
import {environment} from '../../../../../environments/environment';
import {getRouletteBetButtonColor, getRouletteBetButtonLabel} from '../../../utils/roulette.utils';

@Component({
  selector: 'app-roulette-bet-button',
  imports: [],
  templateUrl: './roulette-bet-button.component.html',
  styleUrl: './roulette-bet-button.component.css'
})
export class RouletteBetButtonComponent {
  @Input() betType!: RouletteBetType;
  @Input() betValue!: number[];

  protected readonly getRouletteBetButtonColor = getRouletteBetButtonColor;
  protected readonly getRouletteBetButtonLabel = getRouletteBetButtonLabel;

  private readonly route: ActivatedRoute = inject(ActivatedRoute);
  private readonly http: HttpClient = inject(HttpClient);

  public placeBet(): void {
    const roomId: string | null = this.route.snapshot.paramMap.get('roomId');
    this.http.post(`${environment.apiUrl}/api/roulette/bet?roomId=${roomId}&amount=${10}&betType=${this.betType}`, this.betValue).subscribe()
  }
}
