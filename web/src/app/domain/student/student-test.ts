import { ActiveList } from '../active-list';
import { ExerciseStatus } from './exercise-status';
import { StudentVariant } from './student-variant';

export interface StudentTest extends ActiveList {
  id: string;
  status: ExerciseStatus;
  name: string;
  description: string;
  correctVariant: string;
  variants: StudentVariant[];
}
