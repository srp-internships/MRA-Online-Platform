import { IPageble } from '../core/pagings';
import { BaseEntity } from './base-entity';

export class Documentation extends BaseEntity implements IPageble {
  splitBy: string = '<hr>';

  source(): string {
    return this.content;
  }

  area!: DocumentArea;
  title!: string;
  content!: string;
}

export enum DocumentArea {
  Admin,
  Teacher,
  Student,
}
