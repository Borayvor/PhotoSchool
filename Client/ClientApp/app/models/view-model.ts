import { QuestionBase } from './question-base';

export interface ViewModel {
  readonly allControls: QuestionBase<any>[];
}
