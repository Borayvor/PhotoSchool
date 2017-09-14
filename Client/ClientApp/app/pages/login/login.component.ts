import { FormGroup } from '@angular/forms';
import { QuestionBase } from './../../models/question-base';
import { QuestionControlService } from './../../services/question-control.service';
import { UserQuestionLoginViewModel } from './../../models/user-question-login.view-model';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { AuthService } from './../../services/auth.service';

import { UserAuthModel } from './../../models/user-auth.model';

import { QuestionService } from './../../services/question.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  public viewModel: UserQuestionLoginViewModel;
  public questions: QuestionBase<any>[] = [];
  public form: FormGroup;

  constructor(
    private questionService: QuestionService,
    private qcs: QuestionControlService,
    private authService: AuthService,
    private router: Router
  ) {
    this.viewModel = new UserQuestionLoginViewModel();
    this.questions = questionService.getQuestions(this.viewModel);
  }

  ngOnInit() {
    this.form = this.qcs.toFormGroup(this.questions);
  }

  onSubmit() {
    this.authService.login(this.form.value)
      .subscribe(result => {

        if (result.State === 1) {
          this.router.navigate(['']);
        } else {
          alert(result.Msg);
        }
      });
  }

}
