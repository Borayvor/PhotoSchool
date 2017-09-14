import { FormGroup } from '@angular/forms';
import { Component, Input, DoCheck, OnInit } from '@angular/core';

import { QuestionBase } from './../../models/question-base';

@Component({
  selector: 'app-dynamic-form-question',
  templateUrl: './dynamic-form-question.component.html',
  styleUrls: ['./dynamic-form-question.component.scss']
})
export class DynamicFormQuestionComponent implements OnInit, DoCheck {
  private control: any;

  @Input() question: QuestionBase<any>;
  @Input() form: FormGroup;

  ngOnInit(): void {
    this.control = this.form.get(this.question.key);
  }

  ngDoCheck(): void {
    if (this.control.hasError('required')) {
      this.question.errorMessage = `"${this.question.label}" is required !`;
    }

    if (this.control.hasError('minlength')) {
      this.question.errorMessage = `"${this.question.label}" must be at least ${this.question.minLength} symbols !`;
    }

    if (this.control.hasError('maxlength')) {
      this.question.errorMessage = `"${this.question.label}" must be most ${this.question.maxLength} symbols !`;
    }

    if (this.control.hasError('matchpassword')) {
      this.question.errorMessage = `"${this.question.label}" must be the same as password !`;
    }

  }

}
