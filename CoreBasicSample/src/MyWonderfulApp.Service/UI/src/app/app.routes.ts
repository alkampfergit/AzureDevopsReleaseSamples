import { Routes } from '@angular/router';
import { PageNotFoundComponent } from './components/page-not-found/page-not-found.component';

export const ROUTES: Routes = [
  { path: "page-not-found", component: PageNotFoundComponent },
  { path: "tests", loadChildren: "./tests/tests.module#TestsModule" },
  { path: "**", redirectTo: "page-not-found" }
];
