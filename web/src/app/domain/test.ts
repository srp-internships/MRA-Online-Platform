import { ExcerciseBase } from './exercise-base';
import { List } from './list';
import { Variant } from './variant';

export class Test extends ExcerciseBase implements List {
  variants?: Variant[];
}
