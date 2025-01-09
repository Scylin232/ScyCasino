export enum RoomType {
  Roulette = 0
}

export interface Room {
  id: string,
  name: string,
  roomType: RoomType,
  playerConnections: PlayerConnection
}

export interface PlayerConnection {
  [key: string]: string;
}
