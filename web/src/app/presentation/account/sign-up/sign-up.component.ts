import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Observable } from 'rxjs';
import { AccountRouterLinks, NavigationService } from 'src/app/core/routings';
import { CustomValidationService } from 'src/app/core/services/custom-validation.service';
import { AvaliableCoursesUseCase } from 'src/app/data/usecases/auth/courses.usecase';
import { SignUpUseCase } from 'src/app/data/usecases/auth/sign-up.usecase';
import { List, Student } from 'src/app/domain';

enum Steps {
  UserData = 1,
  ContactData,
  CredentialData,
}

interface RegistrationSteps {
  step: Steps;
  formGroup: FormGroup;
  submitted?: boolean;
}

@Component({
  selector: 'srp-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.scss'],
})
export class SignUpComponent implements OnInit {
  dataForm!: FormGroup;
  contactForm!: FormGroup;
  credentialForm!: FormGroup;

  registrationSteps: RegistrationSteps[] = [];
  selectedStep!: RegistrationSteps;
  steps = Steps;
  courses$: Observable<List[]>;
  firstCourse?: List;
  constructor(
    private _fb: FormBuilder,
    private _customValidator: CustomValidationService,
    private _signUpUseCase: SignUpUseCase,
    private _routingFacade: NavigationService,
    _coursesUseCase: AvaliableCoursesUseCase
  ) {
    this.courses$ = _coursesUseCase.execute();
  }

  ngOnInit(): void {
    this.dataForm = this._fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      occupation: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      phoneNumber: ['', Validators.required],
    });

    this.contactForm = this._fb.group({
      country: ['Таджикистан', Validators.required],
      region: ['', Validators.required],
      city: ['', Validators.required],
      address: ['', Validators.required],
    });

    this.credentialForm = this._fb.group(
      {
        email: ['', [Validators.required, Validators.email]],
        password: ['', Validators.compose([Validators.required, this._customValidator.passwordValidator()])],
        confirmPassword: ['', Validators.required],
        courseId: ['', Validators.required],
        recaptchaToken: ['', Validators.required],
      },
      {
        validators: this._customValidator.matchPassword('password', 'confirmPassword'),
      }
    );

    this.registrationSteps = [
      { step: Steps.UserData, formGroup: this.dataForm },
      { step: Steps.ContactData, formGroup: this.contactForm },
      { step: Steps.CredentialData, formGroup: this.credentialForm },
    ];
    this.selectedStep = this.registrationSteps[0];

    this.courses$.subscribe(c => {
      if (c && c.length) {
        this.credentialForm.get('courseId')?.setValue(c[0].id);
      }
    });
  }

  getControl<T extends keyof Student>(step: Steps, key: T): AbstractControl {
    switch (step) {
      case Steps.UserData:
        return this.dataForm.get(key)!;
      case Steps.ContactData:
        return this.contactForm.get(key)!;
      case Steps.CredentialData:
        return this.credentialForm.get(key)!;
    }
  }

  get passwordMatchError() {
    return this.credentialForm.getError('mismatch');
  }
  get confirmPassword() {
    return this.credentialForm.get('confirmPassword')!;
  }

  onBack() {
    if (this.selectedStep.step !== Steps.UserData) {
      this.selectedStep = this.registrationSteps[+this.selectedStep.step - 2];
    }
  }

  onNext() {
    this.selectedStep.submitted = true;
    if (this.selectedStep.step !== Steps.CredentialData && this.selectedStep.formGroup.valid) {
      this.selectedStep = this.registrationSteps[+this.selectedStep.step];
    }
  }

  onSubmit() {
    this.selectedStep.submitted = true;
    if (!this.credentialForm.valid) return;

    const formValue = { ...this.dataForm.value, ...this.contactForm.value, ...this.credentialForm.value };
    const student: Student = Object.assign(new Student(), formValue);
    this._signUpUseCase.execute(student).subscribe(() => {
      this._routingFacade
        .accountModule(AccountRouterLinks.CheckEmail)
        .extras({ queryParams: { email: student.email } })
        .navigate();
    });
  }
}
