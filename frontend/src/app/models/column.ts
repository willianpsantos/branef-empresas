import { NzTableSortOrder, NzTableSortFn, NzTableFilterList, NzTableFilterFn } from "ng-zorro-antd/table";
import { ColumnDataType } from "./filter";

export type NzEnableEditionFn = (data: any) => boolean;

export interface Column
{
    title: string;
    field: string;
    width?: string;
    sortable?: boolean;
    filter?: boolean;
    dataType?: ColumnDataType;

    pipe?: string;
    format?: string;
}

export interface NzColumn extends Column
{
  sortOrder: NzTableSortOrder | null;
  sortFn: NzTableSortFn<any> | null;
  enableEditionFn?: NzEnableEditionFn,
  sortDirections: NzTableSortOrder[];
  nzAlign?: string;
  className?: string;
}
