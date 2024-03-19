import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {AccountRouterLinks} from 'src/app/core/routings';
import {environment} from "../../../../environments/environment";
import {TokenManagerService} from "../../../core/services/token-storage.service";
import {TokenModel} from "../../../domain";

@Component({
    selector: 'srp-callback',
    templateUrl: './callback.component.html',
    styleUrls: ['./callback.component.scss']
})
export class CallbackComponent implements OnInit {

    links = AccountRouterLinks;

    constructor(
        private _router: Router,
        private _route: ActivatedRoute,
        private _tokenService: TokenManagerService
    ) {
    }

    ngOnInit(): void {
        let accessToken = this._route.snapshot.queryParams['atoken'];
        let refreshToken = this._route.snapshot.queryParams['rtoken'];
        console.log(accessToken);
        console.log(environment.identityClientUrl + "login?callback=" + environment.academyClientUrl + "account/callback");
        if (accessToken == undefined || refreshToken == undefined) {
            window.location.href = environment.identityClientUrl + "login?callback=" + environment.academyClientUrl + "account/callback";
        }
        this._tokenService.remove();
        const myToken: TokenModel = {
            accessToken: accessToken,
            refreshToken: refreshToken,
            isPasswordChanged: false
        };
        this._tokenService.set(myToken);
        this._router.navigateByUrl('/');
    }
}
