import { Pipe, PipeTransform } from "@angular/core";
import { DomSanitizer } from "@angular/platform-browser";

@Pipe({
  name: 'safehtml',
  pure: true
})
export class SafeHtmlPipe implements PipeTransform
{
  constructor(private sanitazer: DomSanitizer) {

  }

  transform(value: any, ...args: any[]) {
    return this.sanitazer.bypassSecurityTrustHtml(value);
  }
}
