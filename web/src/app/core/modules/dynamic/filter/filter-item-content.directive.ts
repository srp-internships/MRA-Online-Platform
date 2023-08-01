import { Directive, TemplateRef } from '@angular/core';

@Directive({ selector: '[srpFilterItemContent]' })
export class FilterItemContentDirective {
  constructor(public templateRef: TemplateRef<any>) {}
}
