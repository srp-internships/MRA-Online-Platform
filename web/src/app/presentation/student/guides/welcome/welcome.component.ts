import { AfterContentInit, Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { GetStudentRatingUseCase } from 'src/app/data/usecases/ratings/get-student-rating.usecase';
import { StudentRating } from 'src/app/domain';
import { LoaderService } from 'src/app/shared/components/loader/loader.service';

@Component({
  selector: 'srp-welcome',
  templateUrl: './welcome.component.html',
  styleUrls: ['./welcome.component.scss'],
})
export class WelcomeComponent implements AfterContentInit {
  rate?: StudentRating;
  constructor(
    private getStudentRatingUseCase: GetStudentRatingUseCase,
    private route: ActivatedRoute,
    private _loader: LoaderService
  ) {}

  ngAfterContentInit(): void {
    const courseId = this.route.snapshot.paramMap.get('courseId')!;
    this._loader.show();
    this.getStudentRatingUseCase.execute(courseId).subscribe(rate => {
      this.rate = rate;
      this._loader.hide();
    });
  }

  getRatePercent(rate: StudentRating): number {
    return rate && rate.totalRate ? (rate.completedRate * 100) / rate.totalRate : 0;
  }
}
