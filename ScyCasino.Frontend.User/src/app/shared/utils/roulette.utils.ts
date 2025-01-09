import {blackRouletteNumbers, redRouletteNumbers, RouletteBetType} from '../models/roulette.model';

export function getRouletteBetButtonColor(betType: RouletteBetType, betValue: number[]): string {
  if (betType == RouletteBetType.Black || (betType == RouletteBetType.Straight && blackRouletteNumbers.includes(betValue[0])))
    return "#41444B";

  if (betType == RouletteBetType.Red || (betType == RouletteBetType.Straight && redRouletteNumbers.includes(betValue[0])))
    return "#FF7457";

  return "#95c361";
}

export function getRouletteBetButtonLabel(betType: RouletteBetType, betValue: number[]): string {
  switch (betType)
  {
    case RouletteBetType.Straight:
      return betValue[0].toString();
    case RouletteBetType.Column:
      return "2to1";
    case RouletteBetType.Range:
      return betValue[0] == 1 ? "1to18" : "19to36";
    case RouletteBetType.Dozen:
      return betValue[0] == 1 ? "1st12" : betValue[0] == 13 ? "2nd12" : "3rd12";
    default:
      return RouletteBetType[betType];
  }
}
