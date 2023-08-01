import { StudentsRating, StudentsRatingModel } from 'src/app/domain';
import { Mapper } from '../base/mapper';

export class StudentsRatingMapper extends Mapper<StudentsRating, StudentsRatingModel> {
  mapFrom(param: StudentsRating): StudentsRatingModel {
    return {
      fullName: param.name,
      totalRate: param.totalRate,
      totalSubmit: param.totalSubmit,
    };
  }
  mapTo(param: StudentsRatingModel): StudentsRating {
    return {
      id: '',
      name: param.fullName,
      totalRate: param.totalRate,
      totalSubmit: param.totalSubmit,
    };
  }
}
