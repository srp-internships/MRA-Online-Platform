import {
  ChangeDetectionStrategy,
  Component,
  ContentChildren,
  DoCheck,
  EventEmitter,
  Input,
  IterableDiffer,
  IterableDiffers,
  Output,
  QueryList,
} from '@angular/core';
import { List } from 'src/app/domain/list';
import { FilterItemDirective } from './filter-item.directive';

@Component({
  selector: 'srp-dynamic-filter',
  templateUrl: './filter.component.html',
  styleUrls: ['./filter.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DynamicFilterComponent implements DoCheck {
  name: string = '';
  iterableDiffer: IterableDiffer<List>;
  @ContentChildren(FilterItemDirective) items!: QueryList<FilterItemDirective>;
  @Input() sourse!: List[];
  @Output() filterChange = new EventEmitter<any[]>();
  constructor(iterableDiffers: IterableDiffers) {
    this.iterableDiffer = iterableDiffers.find([]).create(undefined);
  }
  // eslint-disable-next-line unused-imports/no-unused-vars
  ngDoCheck() {
    const changes = this.iterableDiffer.diff(this.sourse);
    if (changes) {
      this.onListFilter();
    }
  }

  onListFilter() {
    if (this.sourse) {
      this.filterChange.emit(this.sourse.filter(x => x.name.toLowerCase().includes(this.name.toLowerCase())));
    }
  }

  hasFilterItems(): boolean {
    return !!this.items.length;
  }
}
