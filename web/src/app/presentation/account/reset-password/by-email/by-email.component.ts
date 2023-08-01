import { Component, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { AccountRouterLinks, NavigationService } from 'src/app/core/routings';
import { AuthService } from 'src/app/data/services/auth.service';
import { LoaderService } from 'src/app/shared/components/loader/loader.service';

@Component({
  selector: 'srp-by-email',
  templateUrl: './by-email.component.html',
  styleUrls: ['./by-email.component.scss'],
})
export class ByEmailComponent implements OnInit {
  submitted = false;
  emailControl!: FormControl;

  constructor(
    private authService: AuthService,
    private loaderService: LoaderService,
    private _routingFacade: NavigationService
  ) {}

  ngOnInit(): void {
    this.emailControl = new FormControl('', [Validators.required, Validators.email]);
  }

  onSubmit() {
    this.submitted = true;
    if (!this.emailControl.valid) {
      return;
    }

    this.loaderService.show();

    this.authService.forgotPassword(this.emailControl.value).subscribe(() => {
      this.loaderService.hide();
      this._routingFacade
        .accountModule(AccountRouterLinks.CheckEmail)
        .extras({ queryParams: { email: this.emailControl.value } })
        .navigate();
    });
  }
}
