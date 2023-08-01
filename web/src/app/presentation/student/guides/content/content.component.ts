import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { GetStudentTopicUseCase } from 'src/app/data/usecases/topic/get-topic.usecase';
import { Topic } from 'src/app/domain';
import { LoaderService } from 'src/app/shared/components/loader/loader.service';
import { PrintElementService } from 'src/app/core/services/print-element.service';
import { PagingService } from 'src/app/core/pagings';
import { LayoutService } from 'src/app/layout/layout.service';
import { NavigationService } from 'src/app/core/routings';

@Component({
  selector: 'srp-content',
  templateUrl: './content.component.html',
  styleUrls: ['./content.component.scss'],
  providers: [PagingService],
})
export class GuidContentComponent implements OnInit {
  topic: Topic | undefined;
  @ViewChild('content') content!: ElementRef;
  constructor(
    private route: ActivatedRoute,
    private getTopicUseCase: GetStudentTopicUseCase,
    private _navigation: NavigationService,
    private loader: LoaderService,
    private printTopicService: PrintElementService,
    public pagingService: PagingService,
    private _layout: LayoutService
  ) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe(qp => {
      const topicId = qp.get('topicId');
      this.pagingService.clear();
      if (topicId) {
        this.loader.show();
        this.getTopicUseCase.execute(topicId).subscribe(th => {
          this.topic = Object.assign(new Topic(th.courseId), th);
          this.pagingService.build(this.topic);
          this.topic.content = this.pagingService.content(1);
          this.loader.hide();
        });
      }
    });
  }

  onOpenExercise() {
    this._navigation.relative(`../exercises`, this.route).navigate();
  }

  onOpenTest() {
    this._navigation.relative(`../tests`, this.route).navigate();
  }

  onOpenProjectExercise() {
    this._navigation.relative(`../projectExercises`, this.route).navigate();
  }

  printTopic() {
    if (this.topic) {
      this.printTopicService.print(this.content.nativeElement, this.topic.name);
    }
  }

  toHome() {
    this._navigation.relative(`../../`, this.route).navigate();
  }

  onPaginationClick(n: number) {
    this.topic!.content = this.pagingService.content(n);
    this._layout.scrollTop();
  }
}
