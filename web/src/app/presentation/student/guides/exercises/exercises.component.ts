import { Component, Inject, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { finalize } from 'rxjs';
import { CheckExerciseUseCase } from 'src/app/data/usecases/exercises/check-exercise.usecase';
import { GetStudentExercisesUseCase } from 'src/app/data/usecases/exercises/get-exercises.usecase';
import { ExerciseStatus } from 'src/app/domain';
import { List } from 'src/app/domain/list';
import { StudentExercise } from 'src/app/domain/student/student-exercise';
import { BackgroundTaskService } from 'src/app/core/services/background-task.service';
import { SharedData, SHARED_DATA } from 'src/app/shared/shared-data';
import { NavigationService } from 'src/app/core/routings';

@Component({
  selector: 'srp-exercises',
  templateUrl: './exercises.component.html',
  styleUrls: ['./exercises.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class GuideExercisesComponent implements OnInit {
  isSubmitted!: boolean;
  currentTopic: List | undefined;
  exercises!: StudentExercise[];
  defaultTempaltes: string[] = [];
  selectedExercise: StudentExercise | undefined;
  exerciseStatus = ExerciseStatus;

  topicId!: string;
  constructor(
    private checkExerciseUseCase: CheckExerciseUseCase,
    private getThemeExercises: GetStudentExercisesUseCase,
    private route: ActivatedRoute,
    private _navigationService: NavigationService,
    private toastr: ToastrService,
    private backgroundTasks: BackgroundTaskService,
    @Inject(SHARED_DATA) private _sharedData: SharedData
  ) {}

  editorOptions = {
    language: 'csharp',
    minimap: {
      enabled: false,
    },
    contextmenu: false,
    automaticLayout: true,
  };

  async ngOnInit() {
    await this.backgroundTasks.tasks$;
    this.topicId = this.route.snapshot.paramMap.get('topicId')!;
    this.currentTopic = (this._sharedData['topicList'] as List[]).find(x => x.id === this.topicId);
    if (!this.currentTopic) {
      throw new Error('Тема не найдена');
    }
    this.getThemeExercises.execute(this.topicId).subscribe(exercises => {
      if (!exercises.length) {
        this.toastr.info('Здесь нет никаких задач');
        this._navigationService.relative(`../content`, this.route).navigate();
        return;
      }
      this.exercises = exercises;
      this.defaultTempaltes = exercises.map(e => e.template);
      this.onSelectedTab(exercises[0]);
    });
  }

  navigateToTheme(): void {
    this._navigationService.relative(`../content`, this.route).navigate();
  }

  onSelectedTab(exercise: StudentExercise) {
    this.selectedExercise = this.exercises.find(e => e.id === exercise.id);
    this.exercises.forEach(e => (e.isActive = false));
    exercise.isActive = true;
  }

  onSubmit(exercise: StudentExercise) {
    this.isSubmitted = true;
    const model = { id: exercise.id, code: exercise.template };
    this.checkExerciseUseCase
      .execute(model)
      .pipe(finalize(() => (this.isSubmitted = false)))
      .subscribe(response => {
        if (response.internalError) {
          throw new Error(response.errors);
        }

        const index = this.exercises.findIndex(x => x.id === exercise.id);
        if (response.success) {
          this.exercises[index].status = ExerciseStatus.Passed;
          this.toastr.success('Правильный ответ', '', { positionClass: 'toast-bottom-center', timeOut: 1000 });
        } else {
          this.exercises[index].status = ExerciseStatus.Failed;
          this.toastr.error(response.errors, 'Ошибка', {
            positionClass: 'toast-bottom-full-width',
            disableTimeOut: true,
            closeButton: true,
          });
        }
      });
  }

  getTabIconColor(state: ExerciseStatus | undefined): string {
    switch (state) {
      case this.exerciseStatus.Failed:
        return 'has-text-danger';
      case this.exerciseStatus.Passed:
        return 'has-text-success';
      default:
        return '';
    }
  }

  hasUserChanges(): boolean {
    if (!this.exercises) return false;
    for (let i = 0; i < this.exercises.length; i++) {
      const exercise = this.exercises[i];
      const str1 = exercise.template.replace(/(?:\r\n|\r|\n)/g, '');
      const str2 = this.defaultTempaltes[i].replace(/(?:\r\n|\r|\n)/g, '');
      if (str1 !== str2 && exercise.status !== ExerciseStatus.Passed) return true;
    }
    return false;
  }
}
