import { ModulePath } from '../../base-path';
import { StudentRouterLinks, StudentRoutingModule } from './student.rm';

export class GuideRoutingModule extends ModulePath {
  readonly base: string = StudentRouterLinks.Guides;

  constructor(parent: StudentRoutingModule) {
    super(parent);
  }

  override getPath(link?: GuideRouterLinks): string {
    return super.getPath(link);
  }
}

export enum GuideRouterLinks {
  Topic = ':topicId',
  TopicContent = ':topicId/content',
  TopicExercises = ':topicId/exercises',
  TopicTest = ':topicId/tests',
  TopicProjectExercise = ':topicId/projectExercises',
}
