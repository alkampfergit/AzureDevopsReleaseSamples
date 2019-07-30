import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { SupportClient } from 'src/app/services/myWonderfulAppServicesv1.web';

@Component({
  selector: 'app-test-web-api',
  templateUrl: './test-web-api.component.html',
  styleUrls: ['./test-web-api.component.css'],
  providers: [
    SupportClient
  ]
})
export class TestWebApiComponent implements OnInit {

  public txt: string;
  public echo: Observable<string>;

  constructor(
    private testClient: SupportClient
  ) { }

  ngOnInit() {
  }

  public callEcho() {
    this.echo = this.testClient.ping();
  }

}
