import { NgModule } from '@angular/core';
import {
  MdButtonModule,
  MdCheckboxModule,
  MdInputModule,
  MdDatepickerModule,
  MdTabsModule,
  MdCardModule,
  MD_ERROR_GLOBAL_OPTIONS,
  showOnDirtyErrorStateMatcher
} from '@angular/material';

@NgModule({
  imports: [
    MdButtonModule,
    MdCheckboxModule,
    MdInputModule,
    MdDatepickerModule,
    MdTabsModule,
    MdCardModule],
  exports: [
    MdButtonModule,
    MdCheckboxModule,
    MdInputModule,
    MdDatepickerModule,
    MdTabsModule,
    MdCardModule],
  providers: [
    { provide: MD_ERROR_GLOBAL_OPTIONS, useValue: { errorStateMatcher: showOnDirtyErrorStateMatcher } }
  ]
})
export class AngularMaterialModule { }
