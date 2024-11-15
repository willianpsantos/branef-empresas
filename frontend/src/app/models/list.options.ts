import { BodyValueType } from "./body.value.type";
import { QueryFilter } from "./filter";

export class ListOptions
{
  page!: number;
  take!: number;
  filters!: QueryFilter[];

  toBody() {
    const body: { [key: string]: BodyValueType } = {};

    if ((this.filters?.length ?? 0) == 0)
      return body;

    for(let filter of this.filters)
      body[filter.field] = filter.value;

    return body;
  }
}
