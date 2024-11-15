import { ApiResponse } from "./api-response";

export interface PageResponse<TData> extends ApiResponse<TData[]>
{
  page: number,
  take: number,
  count: number,
  data: TData[] | undefined
}
