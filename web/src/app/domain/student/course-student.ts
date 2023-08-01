import { Course } from '../course';

export interface StudentCourse extends Course {
  teacherName: string;
  endDate: Date;
  totalThemes: number;
  completedThemes: number;
}
