import { QuestionControlService } from './../../services/question-control.service';
import { FormGroup } from '@angular/forms';
import { QuestionBase } from './../../models/question-base';
import { UserQuestionRegisterViewModel } from './../../models/user-question-register.view-model';
import { Component, OnInit } from '@angular/core';

import { AuthService } from './../../services/auth.service';
import { QuestionService } from './../../services/question.service';

import { UserAuthModel } from './../../models/user-auth.model';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  public viewModel: UserQuestionRegisterViewModel;
  public questions: QuestionBase<any>[] = [];
  public form: FormGroup;

  constructor(
    private questionService: QuestionService,
    private qcs: QuestionControlService,
    private authService: AuthService) {
    this.viewModel = new UserQuestionRegisterViewModel();
    this.questions = questionService.getQuestions(this.viewModel);
  }

  ngOnInit() {
    this.form = this.qcs.toFormGroup(this.questions);
  }

  onSubmit() {
    this.authService.register(this.form.value)
      .subscribe(result => {
        console.log(result);
      });
  }

}
