import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GamesListComponent } from './games-list/games-list.component';
import { GamesRoutingModule } from './games-routing.module';
import { HttpClientModule } from '@angular/common/http';
import { AppMaterialModule } from '../../app-material/app-material.module';
import { GamesService } from './games.service';
import { AddGameComponent } from './add-game/add-game.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MAT_DATE_LOCALE } from '@angular/material';

@NgModule({
  imports: [
    CommonModule,
    HttpClientModule,
    AppMaterialModule,
    FormsModule,
    ReactiveFormsModule,
    GamesRoutingModule
  ],
  entryComponents:[
    AddGameComponent
  ],
  declarations: [GamesListComponent, AddGameComponent],
  providers: [
    GamesService,
  { provide: MAT_DATE_LOCALE, useValue: 'de-AT'}
  ]
})
export class GamesModule { }
