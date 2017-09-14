import { ViewModel } from './view-model';
import { QuestionTextbox } from './question-textbox';
import { UserQuestionLoginViewModel } from './user-question-login.view-model';
import { QuestionBase } from './question-base';

export class UserQuestionRegisterViewModel extends UserQuestionLoginViewModel implements ViewModel {

  constructor() {
    super();
    this._allControls = this._allControls.concat([
      new QuestionTextbox({
        key: 'confirm-password',
        label: 'Confirm password',
        type: 'password',
        required: true,
        order: 3
      })]);
  }

  public get confirmPassword(): QuestionTextbox {
    return this._allControls[2] as QuestionTextbox;
  }

}
