export function getColorFromUserId(userId: string): string {
  return `#${userId.substring(0,6)}`;
}
