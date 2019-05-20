import { Component, OnInit } from '@angular/core';
import { SupportClient } from 'src/app/services/myWonderfulAppServicesv1.web';

@Component({
  selector: 'app-do-stuff',
  templateUrl: './do-stuff.component.html',
  styleUrls: ['./do-stuff.component.css']
})
export class DoStuffComponent implements OnInit {

  public number: number;
  public result: number;
  public greet: string;

  constructor(
    private supportClient: SupportClient
  ) {
    this.result = 0;
    this.greet = "";
  }

  ngOnInit() {
  }

  doStuff() {
    this.supportClient.doStuff(this.number)
      .subscribe(result => {
        this.greet = result.greet;
        this.result = result.result;
      });
  }

}
