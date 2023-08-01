import { AppRouterLinks } from '../app-router-links';
import { ModulePath } from '../../base-path';
import { Injectable } from '@angular/core';
import { GuideRoutingModule } from './guide.rm';

@Injectable({ providedIn: 'root' })
export class StudentRoutingModule extends ModulePath {
  readonly base: string = AppRouterLinks.Student;
  readonly guide = new GuideRoutingModule(this);

  override getPath(link?: StudentRouterLinks): string {
    return super.getPath(link);
  }
}

export enum StudentRouterLinks {
  Courses = 'courses',
  Guides = 'courses/:courseId',
}
