import { ViewModel } from './../models/view-model';
import { QuestionTextbox } from './../models/question-textbox';
import { QuestionBase } from './../models/question-base';
import { Injectable } from '@angular/core';

@Injectable()
export class QuestionService {

  getQuestions(questionsModel: ViewModel) {

    const questions: QuestionBase<any>[] = questionsModel.allControls;

    return questions.sort((a, b) => a.order - b.order);
  }

}
