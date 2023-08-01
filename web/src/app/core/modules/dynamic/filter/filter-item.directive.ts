import { ContentChild, Directive, Input } from '@angular/core';
import { FilterItemContentDirective } from './filter-item-content.directive';

// eslint-disable-next-line @angular-eslint/directive-selector
@Directive({ selector: 'srp-filter-item' })
export class FilterItemDirective {
  @Input() hasIcon: boolean = true;
  @ContentChild(FilterItemContentDirective) filterItemContent!: FilterItemContentDirective;
}
