import { ToolsService } from './../../common/tools.service';
import { Injectable } from '@angular/core';
import { Player } from './player';

@Injectable()
export class PlayerService {

  constructor(private toolsService: ToolsService) { }

  getPlayers(teamId?: string){

  }
}
