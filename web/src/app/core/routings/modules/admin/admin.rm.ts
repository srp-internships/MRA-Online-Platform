import { AppRouterLinks } from '../app-router-links';
import { ModulePath } from '../../base-path';
import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class AdminRoutingModule extends ModulePath {
  readonly base: string = AppRouterLinks.Admin;

  override getPath(link?: AdminRouterLinks): string {
    return super.getPath(link);
  }
}

export enum AdminRouterLinks {
  Teachers = 'teachers',
  Docs = 'docs',
}
