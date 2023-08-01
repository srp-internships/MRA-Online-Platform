import { List } from '../list';

export interface StudentsRating extends List {
  totalRate: number;
  totalSubmit: number;
}

export interface StudentsRatingModel {
  fullName: string;
  totalRate: number;
  totalSubmit: number;
}
