import { Column } from "./column";

export type ColumnDataType = 'number' | 'string' | 'date';

export interface FilterValue
{
  value: string,
  type: ColumnDataType
}

export class QueryFilter
{
  field: string = "";
  value!: any;

  constructor(data: {
      field: string,
      value: any
    } | undefined = undefined
  ) {
    this.field = data?.field?.toString() ?? "";
    this.value = data?.value;
  }

  static toQueryFilters(
    data: {
      [key: string]: { value: any, type: ColumnDataType }
    }
  )
  : QueryFilter[] | undefined {
    if (!data)
      return undefined;

    const filters: QueryFilter[] = [];

    for(let key in data) {
      let q: QueryFilter = new QueryFilter();

      q.field = key,
      q.value = data[key].value,

      filters.push(q);
    }

    return filters;
  }
}

export interface FilterChangedArgs {
  columns?: Column[],
  filters?: QueryFilter[],
  page?: number,
  take?: number,
  count?: number,
  data?: any[]
}
