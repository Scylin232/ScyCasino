export interface ClaimModel {
  iss: string,
  sub: string,
  aud: string,
  exp: number,
  iat: number,
  auth_time: number,
  acr: string,
  email: string,
  email_verified: boolean,
  given_name: string,
  picture: string,
}
