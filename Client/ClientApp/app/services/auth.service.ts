import { CookieService } from 'angular2-cookie/services/cookies.service';
import { Injectable } from '@angular/core';
import { Headers, Http, Response } from '@angular/http';

import { Observable } from 'rxjs/Observable';

import { RequestResultModel } from './../models/request-result.model';
import { UserAuthModel } from './../models/user-auth.model';

import { HttpService } from './http.service';

import { HttpRequestOptions } from './../models/http-request-options';
import { CookieOptionsArgs } from "angular2-cookie/services/cookie-options-args.model";

@Injectable()
export class AuthService {
  private readonly AUTH_PATH = 'Auth/';
  private readonly tokeyKey = 'token';
  private token: string;

  constructor(
    private http: HttpService,
    private cookieService: CookieService
  ) { }

  register(user: UserAuthModel): Observable<any> {
    return this.http.post(
      this.AUTH_PATH + 'Register',
      { Username: user.username, Password: user.password })
      .map(response => {
        const result = response.json() as RequestResultModel;

        return result;
      });
  }

  login(user: UserAuthModel): Observable<any> {
    return this.http.post(
      this.AUTH_PATH + 'Login',
      { Username: user.username, Password: user.password })
      .map(response => {
        const result = response.json() as RequestResultModel;

        if (result.State === 1) {
          const resultData = result.Data as any;
          const cookieOptions: CookieOptionsArgs = { expires: new Date(resultData.expiresIn) };
          console.log(cookieOptions);

          this.cookieService.put(this.tokeyKey, resultData.accessToken, cookieOptions);
        }

        return result;
      });
  }

  logout(): Observable<any> {
    const header = this.initAuthHeaders();

    return this.http.get(this.AUTH_PATH + 'Logout', null, header)
      .map(response => {
        const result = response.json();
        // this.cookieService.remove(this.tokeyKey);
      });
  }

  checkLogin(): boolean {
    const token = this.cookieService.get(this.tokeyKey);

    if (!token) {
      return false;
    }

    const header = this.initAuthHeaders();

    this.http.get(this.AUTH_PATH + 'GetUserInfo', null, header)
      .subscribe(response => {
        const result = response.json();
        console.log(result);
      },
      err => {
        console.log(err);
      });

    return true;
  }

  private getLocalToken(): string {
    if (!this.token) {
      this.token = this.cookieService.get(this.tokeyKey);
    }
    return this.token;
  }

  private initAuthHeaders(): Headers {
    const token = this.getLocalToken();
    const headers = new Headers();

    if (token) {
      headers.append('Authorization', 'Bearer ' + token);
    }

    return headers;
  }
}
