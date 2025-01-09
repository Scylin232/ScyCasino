import {generateSequenceWithReverseStep} from '../utils/sequence.utils';

export enum RouletteBetType {
  Straight,
  Split,
  Street,
  Corner,
  SixLine,
  Column,
  Dozen,
  Even,
  Odd,
  Range,
  Red,
  Black
}

export interface RouletteBet {
  userId: string,
  amount: number,
  rouletteBetType: RouletteBetType,
  betValues: number[]
}

export const rouletteNumbers: number[] = generateSequenceWithReverseStep(1, 36, 1, 3);

export const blackRouletteNumbers: number[] = [
  2,4,6,8,10,11,13,15,17,20,22,24,26,28,29,31,33,35
];

export const redRouletteNumbers: number[] = [
  1,3,5,7,9,12,14,16,18,19,21,23,25,27,30,32,34,36
]
