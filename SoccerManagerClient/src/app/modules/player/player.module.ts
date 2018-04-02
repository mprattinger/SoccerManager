import { PlayerRoutingModule } from './player-routing.module';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PlayerListComponent } from './player-list/player-list.component';

@NgModule({
  imports: [
    CommonModule,
    PlayerRoutingModule
  ],
  declarations: [PlayerListComponent]
})
export class PlayerModule { }
