import { BaseEntity } from './base-entity';
import { List } from './list';

export abstract class ExcerciseBase extends BaseEntity implements List {
  name!: string;
  description!: string;
  rating!: number;
  constructor(public themeId: string) {
    super();
  }
}
