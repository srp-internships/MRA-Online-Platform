import { Component, Injector, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { BaseService } from '../data/base/base-service';
import { ODATA_SERVICE } from '../data/base/odata-request';
import { AdminService } from '../data/services/admin.service';
import { StudentService } from '../data/services/student.service';
import { TeacherService } from '../data/services/teacher.service';
import { TokenManagerService } from '../core/services/token-storage.service';
import { LayoutService } from '../layout/layout.service';
import { HELPER_SERVICE } from '../data/base/helper-service';
import { NavigationService } from '../core/routings';
import { ChangePasswordMessageComponent } from '../shared/components/change-password-message/change-password-message.component';

export function serviceFactory(tokenService: TokenManagerService, injector: Injector): BaseService {
  const role = tokenService.getRole();
  if (role === 'student') {
    return injector.get(StudentService);
  }
  if (role === 'admin') {
    return injector.get(AdminService);
  }
  return injector.get(TeacherService);
}

@Component({
  selector: 'srp-presentation',
  providers: [
    { provide: ODATA_SERVICE, useFactory: serviceFactory, deps: [TokenManagerService, Injector] },
    { provide: HELPER_SERVICE, useFactory: serviceFactory, deps: [TokenManagerService, Injector] },
    LayoutService,
  ],
  templateUrl: './presentation.component.html',
  styleUrls: ['./presentation.component.scss'],
})
export class PresentationComponent implements OnInit {
  constructor(
    readonly _navigationService: NavigationService,
    private tokenManager: TokenManagerService,
    private toastr: ToastrService,
    layout: LayoutService
  ) {
    layout.onToggleMenu(false);
    this._navigationService.onBasePath.subscribe(() => {
      const role = this.tokenManager.getRole();
      const url = this._navigationService.getUrl();
      if (!url || url === '/') {
        this._navigationService.features(role).navigate();
      }
    });
  }

  ngOnInit() {
    const hasChangedPassword = this.tokenManager.getToken()?.isPasswordChanged;
    const strSetting = localStorage.getItem('srp_settings');
    const settings = strSetting ? JSON.parse(strSetting) : {};
    if (!hasChangedPassword && !settings['disable_ch_p']) {
      this.toastr.show('Мы попросим вас сменить пароль для вашей безопасности!', '', {
        toastComponent: ChangePasswordMessageComponent,
      });
    }
  }
}
