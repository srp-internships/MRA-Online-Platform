import { ActiveList } from '../active-list';
import { ExerciseStatus } from './exercise-status';

export interface StudentExercise extends ActiveList {
  status: ExerciseStatus;
  template: string;
  description: string;
}
