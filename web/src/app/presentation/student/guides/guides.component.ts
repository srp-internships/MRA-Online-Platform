import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { GetStudentTopicsUseCase } from 'src/app/data/usecases/topic/get-topic-list.usecase';
import { TopicList } from 'src/app/domain';
import { BackgroundTaskService } from 'src/app/core/services/background-task.service';
import { LayoutService } from 'src/app/layout/layout.service';
import { SharedData, SHARED_DATA } from 'src/app/shared/shared-data';
import { NavigationService } from 'src/app/core/routings';
import { GuideRouterLinks } from 'src/app/core/routings/modules/student/guide.rm';

@Component({
  selector: 'srp-guides',
  templateUrl: './guides.component.html',
  styleUrls: ['./guides.component.scss'],
})
export class GuidesComponent implements OnInit, OnDestroy {
  subscription!: Subscription;
  constructor(
    private route: ActivatedRoute,
    private getTopicListUseCase: GetStudentTopicsUseCase,
    private layout: LayoutService,
    @Inject(SHARED_DATA) private _sharedData: SharedData,
    private backgroundTasks: BackgroundTaskService,
    private _navigationService: NavigationService
  ) {}
  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
  ngOnInit(): void {
    const courseId = this.route.snapshot.paramMap.get('courseId') ?? '';
    const topics$ = this.getTopicListUseCase.execute(courseId);
    const task$ = new Promise<TopicList[]>((resolve, reject) => {
      this.subscription = topics$.subscribe({
        next: topics => {
          this._sharedData['topicList'] = topics;
          this.layout.setSideNavItems([
            {
              title: 'Темы',
              items: topics.map((g, index) => ({
                link: this._navigationService
                  .guideModule(GuideRouterLinks.Topic)
                  .resolve({ courseId, topicId: g.id })
                  .getPath(),
                text: index + 1 + '.  ' + g.name,
                disabled: !g.isStarted,
                disabledText: `Начало ${new Date(g.startDate).toLocaleDateString()}`,
              })),
            },
          ]);
          resolve(topics);
        },
        error: err => {
          this._navigationService.relative('../', this.route).navigate();
          reject(err);
        },
      });
    });
    this.backgroundTasks.addTasks(task$);
  }
}
