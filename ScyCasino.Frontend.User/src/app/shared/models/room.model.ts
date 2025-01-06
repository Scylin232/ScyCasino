export enum RoomType {
  Roulette
}

export interface Room {
  id: string,
  name: string,
  roomType: RoomType,
}
