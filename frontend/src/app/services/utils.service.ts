import { FormGroup } from "@angular/forms";
import { ValueLabel } from "../models/value-label";
import { HttpParams } from "@angular/common/http";
import { BodyValueType } from "../models/body.value.type";

export default class UtilsService
{
  static emailRegex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;

  static checkEmailFormat(email: string) : boolean {
    return UtilsService.emailRegex.test(email);
  }

  static truncateText(text: string, maxLength: number): string {
    if (!text)
      return "";

    if (text.length <= maxLength)
      return text;

    return text.slice(0, maxLength) + '...';
  }

  static generateUniqueId(): string {
    const timestamp = Date.now().toString(36);
    const randomPart = Math.random().toString(36).substr(2);
    return timestamp + randomPart;
  }

  static markFormAsDirty(form: FormGroup) : void {
    Object.values(form.controls).forEach(control => {
      if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({ onlySelf: true });
      }
    });
  }

  static toValueLabels(data: any[], valueField: string, labelField: string) : ValueLabel[] {
    const values = data.map(d => ({
      value: d[valueField].toString(),
      label: d[labelField].toString()
    } as ValueLabel));

    return values;
  }

  static toValueLabel(data: any, valueField: string, labelField: string) : ValueLabel {
    const value = {
      value: data[valueField].toString(),
      label: data[labelField].toString()
    } as ValueLabel;

    return value;
  }

  static toHttpParams(data:any) : HttpParams {
    const body: { [key:string]: BodyValueType } = {};

    for(let i in data)
      body[i] = data[i];

    return new HttpParams({
      fromObject: body
    })
  }
}
