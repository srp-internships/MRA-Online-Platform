import { Component, Inject, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ExerciseStatus } from 'src/app/domain';
import { List } from 'src/app/domain/list';
import { BackgroundTaskService } from 'src/app/core/services/background-task.service';
import { SharedData, SHARED_DATA } from 'src/app/shared/shared-data';
import { NavigationService } from 'src/app/core/routings';
import { StudentProjectExercise } from 'src/app/domain/student/student-project-exercise';
import { GetStudentProjectExercisesUseCase } from 'src/app/data/usecases/projectExercise/get-student-project-exercise.usecase';
import { SubmitProjectExercisesUseCase } from 'src/app/data/usecases/projectExercise/submit-project-exercise.usecase';
import { ProjectExerciseSubmit } from 'src/app/domain/student/project-exercise-submit';
import { HttpResponse } from '@angular/common/http';
import { finalize } from 'rxjs';

@Component({
  selector: 'srp-student-project-exercise',
  templateUrl: './student-project-exercise.component.html',
  styleUrls: ['./student-project-exercise.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class StudentProjectExerciseComponent implements OnInit {
  isSubmitted!: boolean;
  currentTopic: List | undefined;
  exercises!: StudentProjectExercise[];
  defaultTempaltes: string[] = [];
  selectedExercise: StudentProjectExercise | undefined;
  exerciseStatus = ExerciseStatus;
  submitProjectExercise!: ProjectExerciseSubmit;
  message = '';

  topicId!: string;
  constructor(
    private submitProjectExerciseUseCase: SubmitProjectExercisesUseCase,
    private getThemeExercises: GetStudentProjectExercisesUseCase,
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
    this.getThemeExercises.execute(this.topicId).subscribe(exercises => {
      if (!exercises.length) {
        this.toastr.info('Здесь нет никаких проект');
        this.navigateToTheme();
        return;
      }
      this.exercises = exercises;
      this.onSelectedTab(exercises[0]);
    });
  }

  navigateToTheme(): void {
    this._navigationService.relative(`../content`, this.route).navigate();
  }

  onSelectedTab(exercise: StudentProjectExercise) {
    this.selectedExercise = this.exercises.find(e => e.id === exercise.id);
    this.exercises.forEach(e => (e.isActive = false));
    exercise.isActive = true;
  }

  // On file Select
  onChange(event: any) {
    this.submitProjectExercise = new ProjectExerciseSubmit();
    this.submitProjectExercise.file = event.target.files[0];
    this.submitProjectExercise.id = this.selectedExercise?.id;
    this.toastr.success('Файл указан', '', { positionClass: 'toast-bottom-center', timeOut: 1000 });
  }

  onSubmit(exercise: StudentProjectExercise) {
    if (this.submitProjectExercise === undefined || this.submitProjectExercise.file === undefined) {
      throw new Error('Файл не указан, пожалуйста, загрузите файл');
    }
    this.isSubmitted = true;
    const index = this.exercises.findIndex(s => s.id === exercise.id);
    this.submitProjectExerciseUseCase
      .execute(this.submitProjectExercise!)
      .pipe(finalize(() => (this.isSubmitted = false)))
      .subscribe(
        (event: any) => {
          if (event instanceof HttpResponse) {
            this.message = event.body.message;
            if (event.body.success) {
              this.exercises[index].status = ExerciseStatus.WaitForTeacher;
              this.toastr.success(this.message, '', { positionClass: 'toast-bottom-center', timeOut: 1000 });
            } else {
              this.toastr.error(this.message, 'Ошибка', {
                positionClass: 'toast-bottom-full-width',
                disableTimeOut: true,
                closeButton: true,
              });
            }
          }
        },
        err => {
          this.toastr.error(err.error.errors[0].errorMessage, 'Ошибка', {
            positionClass: 'toast-bottom-full-width',
            disableTimeOut: true,
            closeButton: true,
          });
          this.message = 'Не удалось загрузить проект. Повторите попытку позже.';
          this.submitProjectExercise.file = undefined;
        }
      );
  }

  getTabIconColor(state: ExerciseStatus): string {
    switch (state) {
      case this.exerciseStatus.Failed:
        return 'has-text-danger';
      case this.exerciseStatus.Passed:
        return 'has-text-success';
      case this.exerciseStatus.WaitForStudent:
      case this.exerciseStatus.WaitForTeacher:
        return 'has-text-warning';
      default:
        return '';
    }
  }
}
