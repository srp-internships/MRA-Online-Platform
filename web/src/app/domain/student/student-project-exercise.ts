import { ActiveList } from '../active-list';
import { ExerciseStatus } from './exercise-status';

export interface StudentProjectExercise extends ActiveList {
  status: ExerciseStatus;
  description: string;
  comment: string;
  commentDate: string;
}
