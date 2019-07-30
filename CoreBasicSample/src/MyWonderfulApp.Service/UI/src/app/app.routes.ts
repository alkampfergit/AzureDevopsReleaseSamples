import { Routes } from '@angular/router';
import { PageNotFoundComponent } from './components/page-not-found/page-not-found.component';
import { DoStuffComponent } from './pages/do-stuff/do-stuff.component';

export const ROUTES: Routes = [
  { path: "doStuff", component: DoStuffComponent },
  { path: "page-not-found", component: PageNotFoundComponent },
  { path: "tests", loadChildren: "./tests/tests.module#TestsModule" },
  { path: "**", redirectTo: "doStuff" }
];
