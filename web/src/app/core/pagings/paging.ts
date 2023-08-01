import { KeyValue } from '@angular/common';

export interface Paging extends KeyValue<number, string> {
  active: boolean;
}
