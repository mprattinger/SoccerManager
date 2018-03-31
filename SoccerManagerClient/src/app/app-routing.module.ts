import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeLayoutComponent } from './layouts/home-layout.component';
import { LoginLayoutComponent } from './layouts/login-layout.component';
import { AuthGuard } from './auth/auth.guard';
import { LoginComponent } from './login/login.component';
import { HomeComponent } from './home/home.component';

//loadChildren: 'app/modules/home/home.module#HomeModule'
// canActivate: [AuthGuard]
const routes: Routes = [
  { path: '', component: HomeLayoutComponent, children: [
    { path: '', component: HomeComponent },
    { path: 'games', loadChildren: 'app/modules/games/games.module#GamesModule' }
  ] },
  {
    path: '', component: LoginLayoutComponent, children: [
      { path: 'login', component: LoginComponent }
    ]
  },
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
