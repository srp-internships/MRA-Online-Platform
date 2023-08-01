import { ExcerciseBase } from './exercise-base';
import { List } from './list';

export class ProjectExercise extends ExcerciseBase implements List {
  constructor(themeId: string) {
    super(themeId);
  }
}
