import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { GamesListComponent } from './games-list/games-list.component';
import { AddGameComponent } from './add-game/add-game.component';

const routes: Routes = [
  { path: 'list', component: GamesListComponent },
  { path: 'add', component: AddGameComponent}
];

@NgModule({
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class GamesRoutingModule { }
