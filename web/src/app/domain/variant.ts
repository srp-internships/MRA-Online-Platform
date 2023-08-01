import { BaseEntity } from './base-entity';

export class Variant extends BaseEntity {
  testId!: string;
  value!: string;
  isCorrect!: boolean;
}
