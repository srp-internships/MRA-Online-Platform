import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'srp-protected-page',
  templateUrl: './protected-page.component.html',
  styleUrls: ['./protected-page.component.scss'],
})
export class ProtectedPageComponent implements OnInit {
  returnUrl!: string;
  constructor(private route: ActivatedRoute) {}

  ngOnInit() {
    this.route.queryParams.subscribe(param => {
      this.returnUrl = param['returnUrl'];
    });
  }
}
