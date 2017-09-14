import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AngularMaterialModule } from './../angular_material/angular-material.module';

import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { DynamicFormComponent } from './../page_components/dynamic-form/dynamic-form.component';
import { DynamicFormQuestionComponent } from './../page_components/dynamic-form-question/dynamic-form-question.component';

@NgModule({
  imports: [
    CommonModule,
    ReactiveFormsModule,
    AngularMaterialModule
  ],
  declarations: [
    HomeComponent,
    LoginComponent,
    RegisterComponent,
    DynamicFormQuestionComponent,
    DynamicFormComponent
  ]
})
export class AppPagesModule { }
