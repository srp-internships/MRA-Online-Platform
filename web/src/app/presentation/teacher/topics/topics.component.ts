import { Component, Inject, OnInit } from '@angular/core';
import { Topic } from 'src/app/domain';
import { List } from 'src/app/domain/list';
import { ConfirmDialogComponent } from 'src/app/shared/components/confirm-dialog/confirm-dialog.component';
import { CRUDBaseComponent } from '../../crud-base-component';
import { TopicsFormControlService } from './topics-control.service';
import { TopicUseCasesFacade } from './topic-uasecases.facade';
import { ActivatedRoute } from '@angular/router';
import { LoaderService } from 'src/app/shared/components/loader/loader.service';
import { CustomRxJsOperators } from 'src/app/core/services/custom-rxjs-operators.service';
import { NavigationService, TeacherRouterLinks } from 'src/app/core/routings';
import { SrpDialogService } from 'src/app/core/modules/srp-dialog';
import { FormStrategyModel, FORM_STRATEGY } from 'src/app/core/form-elements';
import { DynamicDialogData } from 'src/app/core/modules/dynamic/dialog';

@Component({
  selector: 'srp-topics',
  providers: [TopicsFormControlService, TopicUseCasesFacade],
  templateUrl: './topics.component.html',
  styleUrls: ['./topics.component.scss'],
})
export class TopicsComponent extends CRUDBaseComponent<Topic> implements OnInit {
  courses!: List[];
  filteredTopics!: Topic[];
  selectedCourseId!: string;
  constructor(
    private _topicUseCasesFacade: TopicUseCasesFacade,
    private _route: ActivatedRoute,
    private _customOperators: CustomRxJsOperators,
    private _loader: LoaderService,
    private _navigationService: NavigationService,
    _dialogService: SrpDialogService,
    formControlService: TopicsFormControlService,
    @Inject(FORM_STRATEGY) formStrategy: FormStrategyModel
  ) {
    formStrategy.model = formControlService;
    super(_dialogService);
  }

  ngOnInit(): void {
    this._loader.show();
    this._topicUseCasesFacade.getCoursesUseCase
      .execute()
      .pipe(
        this._customOperators.navigateIfEmpty(
          this._navigationService.teacherModule(TeacherRouterLinks.Courses),
          `У вас нет курсов для загрузки. Сначала создайте курс!`
        )
      )
      .subscribe(courses => {
        this.courses = courses;
        this.selectedCourseId = this._route.snapshot.queryParams['courseId'] ?? this.courses[0].id;
        this.loadTopics();
      });
  }

  loadTopics() {
    this._loader.show();
    this._topicUseCasesFacade.getUseCase.execute(this.selectedCourseId).subscribe(topics => {
      this.items = topics;
      this._loader.hide();
    });
  }

  onAdd(): any {
    super
      .openDynamicDailog(
        new DynamicDialogData(new Topic(this.selectedCourseId), {
          dialogTitle: 'Новая тема',
          useCase: this._topicUseCasesFacade.createUseCase,
          primaryButtonText: 'Сохранить',
        })
      )
      .subscribe(topic => {
        this.items.push(topic);
      });
  }

  onEdit(item: Topic): void {
    super
      .openDynamicDailog(
        new DynamicDialogData(Object.assign(new Topic(item.courseId), item), {
          dialogTitle: 'Изменить тему',
          primaryButtonText: 'Редактировать',
          useCase: this._topicUseCasesFacade.editUseCase,
        })
      )
      .subscribe(topic => {
        this.items[this.items.findIndex(x => x.id == topic.id)] = topic;
      });
  }

  onDelete(topic: Topic): void {
    this.dialogService.open(ConfirmDialogComponent, { data: topic.name }).afterClosed.subscribe((flag: boolean) => {
      if (flag) {
        this._topicUseCasesFacade.deleteUseCase.execute(topic.id).subscribe(() => {
          const index = this.items.findIndex(x => x.id == topic.id);
          this.items.splice(index, 1);
        });
      }
    });
  }

  onNavigateToTasks(topic: List) {
    this._navigationService
      .teacherModule(TeacherRouterLinks.Tasks)
      .extras({ queryParams: { name: topic.name } })
      .resolve({ topicId: topic.id })
      .navigate();
  }
}
