import { Injectable } from '@angular/core';
import { IPageble } from './pageble';
import { Paging } from './paging';

@Injectable()
export class PagingService {
  private _pagings: Paging[] = [];

  get pagings(): Paging[] {
    return this._pagings;
  }

  build<T extends IPageble>(entity: T): void {
    this._pagings = entity
      .source()
      .split(entity.splitBy)
      .map((c, i) => ({ key: i + 1, value: c, active: false }));
  }

  private active(item: Paging) {
    for (const pageing of this._pagings) {
      pageing.active = false;
      if (pageing.key === item.key) {
        pageing.active = true;
      }
    }
  }

  content(n: number) {
    const item = this._pagings.find(x => x.key === n) || this._pagings[0];
    this.active(item);
    return item.value;
  }

  clear() {
    this._pagings = [];
  }
}
