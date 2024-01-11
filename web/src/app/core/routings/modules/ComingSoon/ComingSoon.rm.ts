import { AppRouterLinks } from '../app-router-links';
import { ModulePath } from '../../base-path';
import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class ComingSoonModule extends ModulePath {
  readonly base: string = AppRouterLinks.ComingSoon;

  override getPath(link?: ComingSoonLinks): string {
    return super.getPath(link);
  }
}

export enum ComingSoonLinks {
  Home=''
}
