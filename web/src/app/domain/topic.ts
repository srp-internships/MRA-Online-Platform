import { BaseEntity } from './base-entity';
import { List } from './list';
import { IPageble } from '../core/pagings/pageble';

export class Topic extends BaseEntity implements List, IPageble {
  name!: string;
  startDate: Date = new Date();
  endDate: Date = new Date();
  content?: string;
  haveTests?: boolean;
  haveExercises?: boolean;
  haveProjectExercise?: boolean;

  constructor(public courseId: string) {
    super();
  }
  splitBy: string = '<hr>';
  source(): string {
    return this.content || '';
  }
}
