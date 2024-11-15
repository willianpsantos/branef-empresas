export interface ApiResponse<TData>
{
  success?: boolean,
  message?: string,
  status?: number,
  data?: TData | undefined
}
