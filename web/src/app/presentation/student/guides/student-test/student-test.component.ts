import { Component, Inject, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { finalize } from 'rxjs';
import { ExerciseStatus } from 'src/app/domain';
import { List } from 'src/app/domain/list';
import { BackgroundTaskService } from 'src/app/core/services/background-task.service';
import { SharedData, SHARED_DATA } from 'src/app/shared/shared-data';
import { NavigationService } from 'src/app/core/routings';
import { StudentTest } from 'src/app/domain/student/student-test';
import { CheckTestUseCase } from 'src/app/data/usecases/tests/check-test.usecase';
import { GetStudentTestsUseCase } from 'src/app/data/usecases/tests/get-tests.usecase';
import { StudentVariant } from 'src/app/domain/student/student-variant';

@Component({
  selector: 'srp-student-test',
  templateUrl: './student-test.component.html',
  styleUrls: ['./student-test.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class StudentTestComponent implements OnInit {
  isSubmitted!: boolean;
  currentTopic: List | undefined;
  tests!: StudentTest[];
  selectedTest: StudentTest | undefined;
  selectedVariant: StudentVariant | undefined;
  testStatus = ExerciseStatus;

  topicId!: string;
  constructor(
    private checkTestUseCase: CheckTestUseCase,
    private getThemeTests: GetStudentTestsUseCase,
    private route: ActivatedRoute,
    private _navigationService: NavigationService,
    private toastr: ToastrService,
    private backgroundTasks: BackgroundTaskService,
    @Inject(SHARED_DATA) private _sharedData: SharedData
  ) {}

  async ngOnInit() {
    await this.backgroundTasks.tasks$;
    this.topicId = this.route.snapshot.paramMap.get('topicId')!;
    this.currentTopic = (this._sharedData['topicList'] as List[]).find(x => x.id === this.topicId);
    if (!this.currentTopic) {
      throw new Error('Тема не найдена');
    }
    this.getThemeTests.execute(this.topicId).subscribe(tests => {
      if (!tests.length) {
        this.toastr.info('Здесь нет никаких тесты');
        this.navigateToTheme();
        return;
      }
      this.tests = tests;
      this.onSelectedTab(tests[0]);
    });
  }

  navigateToTheme(): void {
    this._navigationService.relative(`../content`, this.route).navigate();
  }

  onSelectedTab(test: StudentTest) {
    this.selectedTest = this.tests.find(e => e.id === test.id);
    this.tests.forEach(e => (e.isActive = false));
    test.isActive = true;
  }

  onSelectVariant(position: number): void {
    this.selectedVariant = this.selectedTest?.variants[position];
  }

  onSubmit(test: StudentTest, variant?: StudentVariant) {
    if (variant != null) {
      this.isSubmitted = true;
      const model = { testId: test.id, variantId: variant.id };
      this.checkTestUseCase
        .execute(model)
        .pipe(finalize(() => (this.isSubmitted = false)))
        .subscribe(response => {
          const index = this.tests.findIndex(x => x.id === test.id);
          if (response.success) {
            test.correctVariant = variant.value;
            this.tests[index].status = ExerciseStatus.Passed;
            this.toastr.success('Правильный ответ', '', { positionClass: 'toast-bottom-center', timeOut: 1000 });
          } else {
            this.tests[index].status = ExerciseStatus.Failed;
            this.toastr.error('Неправильный ответ', 'Ошибка', {
              positionClass: 'toast-bottom-center',
              timeOut: 1000,
            });
          }
        });
    } else {
      this.toastr.error('Пожалуйста, выберите вариант', 'Ошибка', {
        positionClass: 'toast-bottom-center',
        timeOut: 1000,
      });
    }
  }

  getTabIconColor(state: ExerciseStatus | undefined): string {
    switch (state) {
      case this.testStatus.Failed:
        return 'has-text-danger';
      case this.testStatus.Passed:
        return 'has-text-success';
      default:
        return '';
    }
  }
}
