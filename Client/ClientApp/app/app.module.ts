import { QuestionService } from './services/question.service';
import { QuestionControlService } from './services/question-control.service';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpModule, Headers } from '@angular/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppRoutingModule } from './app-routing.module';
import { AppPagesModule } from './pages/app-pages.module';
import { AngularMaterialModule } from './angular_material/angular-material.module';

import { HttpService } from './services/http.service';
import { CookieService } from 'angular2-cookie/services/cookies.service';
import { AuthService } from './services/auth.service';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './page_components/navmenu/navmenu.component';

import { EqualValidatorDirective } from './directives/EqualValidator.directive';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    EqualValidatorDirective
  ],
  imports: [
    BrowserModule,
    HttpModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    AppPagesModule,
    AngularMaterialModule
  ],
  providers: [
    HttpService,
    CookieService,
    AuthService,
    QuestionService,
    QuestionControlService],
  bootstrap: [AppComponent]
})
export class AppModule { }
