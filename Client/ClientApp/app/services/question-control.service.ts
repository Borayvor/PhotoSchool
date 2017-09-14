import { Injectable } from '@angular/core';
import { FormControl, FormGroup, Validators, AsyncValidatorFn, FormBuilder, ValidatorFn, AbstractControl } from '@angular/forms';

import { QuestionBase } from './../models/question-base';

@Injectable()
export class QuestionControlService {

  constructor(private fb: FormBuilder) { }

  toFormGroup(questions: QuestionBase<any>[]) {
    const group: any = {};

    questions.forEach(question => {
      group[question.key] = question.required ? this.fb.control(
        question.value || '',
        [
          Validators.required,
          Validators.minLength(question.minLength),
          Validators.maxLength(question.maxLength)
        ])
        : this.fb.control(question.value || '');
    });

    const fbGroup = this.fb.group(group, {
      validator: this.passwordsEqualValidator()
    });

    return fbGroup;
  }

  private passwordsEqualValidator(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } => {
      if (!control.get('password') || !control.get('confirm-password')) {
        return null;
      }

      const password = control.get('password').value;
      const confirmPassword = control.get('confirm-password').value;

      if (password !== confirmPassword) {
        control.get('confirm-password').setErrors({ matchpassword: true });
      } else {
        return null;
      }
    };
  }

}
