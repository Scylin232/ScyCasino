export interface PaginatedResponse<T> {
  totalPages: number,
  totalCount: number,
  results: T[]
}

export interface ErrorResponse {
  code: string;
  message: string;
}
