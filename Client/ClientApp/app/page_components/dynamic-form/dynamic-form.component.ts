import { FormGroup } from '@angular/forms';
import { Component, OnInit, Input } from '@angular/core';

import { QuestionControlService } from './../../services/question-control.service';
import { QuestionBase } from './../../models/question-base';

@Component({
  selector: 'app-dynamic-form',
  templateUrl: './dynamic-form.component.html',
  styleUrls: ['./dynamic-form.component.scss']
})
export class DynamicFormComponent implements OnInit {

  @Input() questions: QuestionBase<any>[] = [];
  public form: FormGroup;

  constructor(private qcs: QuestionControlService) { }

  ngOnInit() {
    this.form = this.qcs.toFormGroup(this.questions);
  }

}
