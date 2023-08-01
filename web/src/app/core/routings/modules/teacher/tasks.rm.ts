import { AppRouterLinks } from '../app-router-links';
import { ModulePath } from '../../base-path';

export class TasksRoutingModule extends ModulePath {
  readonly base: string = AppRouterLinks.Teacher;

  override getPath(link?: TasksRouterLinks): string {
    return super.getPath(link);
  }
}

export enum TasksRouterLinks {
  Tests = 'Тесты',
  Exercises = 'Упражнения',
}
