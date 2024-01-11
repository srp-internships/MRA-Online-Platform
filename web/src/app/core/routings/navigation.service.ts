import { Injectable } from '@angular/core';
import { Location } from '@angular/common';
import { ActivatedRoute, NavigationEnd, NavigationExtras, Router } from '@angular/router';
import {
  AccountRouterLinks,
  AccountRoutingModule,
  ResetPassworRouterLinks,
  TeacherRouterLinks,
  TeacherRoutingModule,
} from 'src/app/core/routings';
import { SignUpRouterLinks } from './modules/account/sign-up.rm';
import { AdminRouterLinks, AdminRoutingModule } from './modules/admin/admin.rm';
import { PathResolver } from './path-resolver';
import { StudentRouterLinks, StudentRoutingModule } from './modules/student/student.rm';
import { GuideRouterLinks } from './modules/student/guide.rm';
import { Observable, Subject } from 'rxjs';
import {ComingSoonLinks, ComingSoonModule} from "./modules/ComingSoon/ComingSoon.rm";

@Injectable({
  providedIn: 'root',
})
export class NavigationService {
  private readonly history: string[] = [];
  private _onBasePath: Subject<void> = new Subject<void>();
  readonly onBasePath: Observable<void> = this._onBasePath.asObservable();
  constructor(
    private router: Router,
    private accountRouting: AccountRoutingModule,
    private ComingSoonRouting: ComingSoonModule,
    private teacherRouting: TeacherRoutingModule,
    private adminRouting: AdminRoutingModule,
    private studentRouting: StudentRoutingModule,
    private location: Location
  ) {
    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.history.push(event.urlAfterRedirects);
        if (event.urlAfterRedirects === '/') {
          this._onBasePath.next();
        }
      }
    });
  }

  getUrl(): string {
    return this.router.url;
  }

  signUpModule(link?: SignUpRouterLinks) {
    return new RouterNavigation(this.accountRouting.signUp.getPath(link), this.router);
  }

  resetPasswordModule(link?: ResetPassworRouterLinks) {
    return new RouterNavigation(this.accountRouting.resetPassword.getPath(link), this.router);
  }

  accountModule(link?: AccountRouterLinks) {
    return new RouterNavigation(this.accountRouting.getPath(link), this.router);
  }
  ComingSoonModule(link?: ComingSoonLinks) {
    return new RouterNavigation(this.ComingSoonRouting.getPath(link), this.router);
  }

  teacherModule(link?: TeacherRouterLinks) {
    return new RouterNavigation(this.teacherRouting.getPath(link), this.router);
  }

  adminModule(link?: AdminRouterLinks) {
    return new RouterNavigation(this.adminRouting.getPath(link), this.router);
  }

  studentModule(link?: StudentRouterLinks) {
    return new RouterNavigation(this.studentRouting.getPath(link), this.router);
  }

  guideModule(link?: GuideRouterLinks) {
    return new RouterNavigation(this.studentRouting.guide.getPath(link), this.router);
  }

  relative(relativePath: string, routeState: ActivatedRoute) {
    return new RouterNavigation(relativePath, this.router, { relativeTo: routeState });
  }

  back(): void {
    this.history.pop();
    if (this.history.length > 0) {
      this.location.back();
    } else {
      this.router.navigateByUrl('/');
    }
  }

  features(role: string) {
    switch (role) {
      case 'student':
        return this.studentModule();
      case 'teacher':
        return this.teacherModule();
      case 'admin':
        return this.adminModule();
      default:
        return this.accountModule(AccountRouterLinks.SignIn);
    }
  }
}

export class RouterNavigation {
  constructor(
    private readonly _path: string,
    private readonly _router: Router,
    private readonly _extras?: NavigationExtras
  ) {}

  extras(extras: NavigationExtras) {
    return new RouterNavigation(this._path, this._router, extras);
  }

  getPath(): string {
    return this._path;
  }

  resolve(data: Record<string, string>) {
    return new RouterNavigation(new PathResolver(this._path).resolve(data), this._router, this._extras);
  }

  navigate() {
    return this._router.navigate([this._path], this._extras);
  }
}
