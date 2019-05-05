import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TestsIndexComponent } from './tests-index/tests-index.component';
import { RouterModule } from '@angular/router';
import { TestWebApiComponent } from './test-web-api/test-web-api.component';
import { FormsModule } from '@angular/forms';
import { ROUTES } from './tests.routes';

@NgModule({
  declarations: [
    TestsIndexComponent,
    TestWebApiComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    RouterModule.forChild(ROUTES)
  ]
})
export class TestsModule { }
