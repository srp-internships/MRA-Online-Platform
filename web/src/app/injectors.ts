import { InjectionToken } from '@angular/core';

export const BASE_API_URL = new InjectionToken<string>('url for api address');


import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';
import {environment} from "../environments/environment";

@Injectable()
export class ComingSoonInterceptor implements HttpInterceptor {
    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        if (environment.IsComingSoonMode) {
            window.location.href = '/comingsoon';
        }
        console.log(window.location.href);
        console.log("asdf;hjasuoe;wfiuhw;oeifuhaj;weiuorhfjn;oiewuhfj;oawiehf;owiejfo;iwajeofiweoijoeiwoaiejfoasdhfhfiuehwfkjashlfakjhdflakjdhflaksjdhflaksdjfh");

        return next.handle(request);
    }
}