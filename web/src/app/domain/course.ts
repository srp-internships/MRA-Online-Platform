import { BaseEntity } from './base-entity';
import { List } from './list';

export class Course extends BaseEntity implements List {
  name!: string;
  courseLanguage!: string;
}
