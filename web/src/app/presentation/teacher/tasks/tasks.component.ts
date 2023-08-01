import { Component, Inject, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { filter, forkJoin } from 'rxjs';
import { FormStrategyModel, FORM_STRATEGY } from 'src/app/core/form-elements';
import { DynamicDialogComponent, DynamicDialogData } from 'src/app/core/modules/dynamic/dialog';
import { SrpDialogService } from 'src/app/core/modules/srp-dialog';
import { UseCase } from 'src/app/data/base/use-case';
import { CreateExerciseUseCase } from 'src/app/data/usecases/exercises/create-exercise.usecase';
import { GetTeacherExercisesUseCase } from 'src/app/data/usecases/exercises/get-teacher-exercises.usecase';
import { CreateProjectExerciseUseCase } from 'src/app/data/usecases/projectExercise/create-projectExercise.usecase';
import { GetProjectExercisesUseCase } from 'src/app/data/usecases/projectExercise/get-projectExerise.usecase';
import { CreateTestUseCase } from 'src/app/data/usecases/tests/create-test.usecase';
import { GetTeacherTestsUseCase } from 'src/app/data/usecases/tests/get-teacher-tests.usecase';
import { Excercise, ExcerciseBase, Test } from 'src/app/domain';
import { ProjectExercise } from 'src/app/domain/project-exercise';
import { LoaderService } from 'src/app/shared/components/loader/loader.service';
import { ExercisesFormControlService } from './exercises/exercises-control.service';
import { ProjectExerciseFormControlService } from './project-exercises/project-exercise-control.service';
import { TestsFormControlService } from './tests/tests-control.service';

@Component({
  selector: 'srp-tasks',
  templateUrl: './tasks.component.html',
  providers: [ExercisesFormControlService, TestsFormControlService, ProjectExerciseFormControlService],
})
export class TasksComponent implements OnInit {
  readonly title: string;
  readonly topicId: string;

  excercises!: Excercise[];
  tests!: Test[];
  projectExercises!: ProjectExercise[];
  private base: ExcerciseBase;
  private createUseCase: UseCase<ExcerciseBase, string>;
  constructor(
    readonly _route: ActivatedRoute,
    @Inject(FORM_STRATEGY) readonly _formStrategy: FormStrategyModel,
    private readonly _exerciseForm: ExercisesFormControlService,
    private readonly _testForm: TestsFormControlService,
    private readonly _projectExerciseForm: ProjectExerciseFormControlService,
    private readonly _srpDialogService: SrpDialogService,
    private readonly _getTestUseCase: GetTeacherTestsUseCase,
    private readonly _getExerciseUseCase: GetTeacherExercisesUseCase,
    private readonly _getProjectExerciseUseCase: GetProjectExercisesUseCase,
    private readonly _createExerciseUseCase: CreateExerciseUseCase,
    private readonly _createTestsUseCase: CreateTestUseCase,
    private readonly _createProjectExerciseUseCase: CreateProjectExerciseUseCase,
    private readonly _loader: LoaderService
  ) {
    this.topicId = _route.snapshot.paramMap.get('topicId')!;
    this.title = _route.snapshot.queryParamMap.get('name') ?? 'Задачи темы';
    this.base = new Excercise(this.topicId);
    this.createUseCase = this._createExerciseUseCase;
  }
  ngOnInit(): void {
    this._formStrategy.model = this._exerciseForm;
    this._loader.show();
    forkJoin({
      tests: this._getTestUseCase.execute(this.topicId),
      exercises: this._getExerciseUseCase.execute(this.topicId),
      projectExercises: this._getProjectExerciseUseCase.execute(this.topicId),
    }).subscribe(data => {
      this.excercises = data.exercises;
      this.tests = data.tests;
      this.projectExercises = data.projectExercises;
      this._loader.hide();
    });
  }

  onAdd(): void {
    this._srpDialogService
      .open(DynamicDialogComponent, {
        data: new DynamicDialogData(this.base, {
          dialogTitle: 'Добавить',
          useCase: this.createUseCase,
          primaryButtonText: 'Сохранить',
        }),
      })
      .afterClosed.pipe(filter(x => x))
      .subscribe((item: ExcerciseBase) => {
        if (item instanceof Test) {
          this.tests.push(item);
        }

        if (item instanceof Excercise) {
          this.excercises.push(item);
        }

        if (item instanceof ProjectExercise) {
          this.projectExercises.push(item);
        }
      });
  }

  onSelectedTab(base: ExcerciseBase) {
    this.base = base;
    if (this.base instanceof Excercise) {
      this._formStrategy.model = this._exerciseForm;
      this.createUseCase = this._createExerciseUseCase;
    }

    if (this.base instanceof Test) {
      this._formStrategy.model = this._testForm;
      this.createUseCase = this._createTestsUseCase;
    }

    if (this.base instanceof ProjectExercise) {
      this._formStrategy.model = this._projectExerciseForm;
      this.createUseCase = this._createProjectExerciseUseCase;
    }
  }

  get newExercise(): Excercise {
    return new Excercise(this.topicId);
  }

  get newTest(): Test {
    return new Test(this.topicId);
  }

  get newProjectExercise(): ProjectExercise {
    return new ProjectExercise(this.topicId);
  }
}
