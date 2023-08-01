import { Component, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountRouterLinks, NavigationService } from 'src/app/core/routings';

@Component({
  selector: 'srp-check-email-message',
  templateUrl: './check-email-message.component.html',
})
export class CheckEmailMessageComponent implements OnInit {
  email: string | null = '';

  constructor(
    private route: ActivatedRoute,
    private toastr: ToastrService,
    private _navigationFactory: NavigationService
  ) {}
  ngOnInit(): void {
    this.email = this.route.snapshot.queryParamMap.get('email');
    const emailControl = new FormControl(this.email, [Validators.required, Validators.email]);

    if (emailControl.errors) {
      this.toastr.error('Неверная email. Повторите еще раз!');
      this._navigationFactory.accountModule(AccountRouterLinks.SignIn).navigate();
    }
  }
}
