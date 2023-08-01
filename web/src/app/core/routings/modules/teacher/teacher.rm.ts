import { AppRouterLinks } from '../app-router-links';
import { ModulePath } from '../../base-path';
import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class TeacherRoutingModule extends ModulePath {
  readonly base: string = AppRouterLinks.Teacher;

  override getPath(link?: TeacherRouterLinks): string {
    return super.getPath(link);
  }
}

export enum TeacherRouterLinks {
  Courses = 'courses',
  Topics = 'topics',
  Tasks = 'tasks/:topicId',
  StudentRatings = 'student-ratings',
}
