import { ExcerciseBase } from './exercise-base';
import { List } from './list';

export class Excercise extends ExcerciseBase implements List {
  template!: string;
  test!: string;
  constructor(themeId: string) {
    super(themeId);
  }
}
