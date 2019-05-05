import { Routes } from '@angular/router';
import { TestsIndexComponent } from './tests-index/tests-index.component';
import { TestWebApiComponent } from './test-web-api/test-web-api.component';

export const ROUTES: Routes = [
  {
    path: "",
    children: [
      { path: "test-web-api", component: TestWebApiComponent },
      { path: "", component: TestsIndexComponent },
    ]
  }
];
