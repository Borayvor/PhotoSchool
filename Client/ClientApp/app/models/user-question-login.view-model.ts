import { ViewModel } from './view-model';
import { QuestionTextbox } from './question-textbox';
import { QuestionBase } from './question-base';

export class UserQuestionLoginViewModel implements ViewModel {

  protected _allControls: QuestionBase<any>[];

  constructor() {
    this._allControls = [
      new QuestionTextbox({
        key: 'username',
        label: 'Username',
        required: true,
        minLength: 2,
        order: 1
      }),
      new QuestionTextbox({
        key: 'password',
        label: 'Password',
        type: 'password',
        required: true,
        minLength: 2,
        order: 2
      })
    ];
  }

  public get allControls(): QuestionBase<any>[] {
    return this._allControls;
  }

  public get username(): QuestionTextbox {
    return this.allControls[0] as QuestionTextbox;
  }

  public get password(): QuestionTextbox {
    return this.allControls[1] as QuestionTextbox;
  }

}
