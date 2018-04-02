import { ToolsService } from './../../common/tools.service';
import { Injectable } from '@angular/core';
import { Game } from './game';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class GamesService {

  constructor(private httpClient: HttpClient, private toolsService: ToolsService) { }

  getGames(teamId: string) {
    return Observable.create(obs => {
      obs.next(this.games.filter(e => e.teamId === teamId));
      obs.complete();
    });
  }

  addGame(game: Game):boolean{
    let guid = this.toolsService.createGuid();
    game.gameId = guid;
    this.games.push(game);
    return true;
  }

  changeGame(oldGame: Game, game: Game):boolean{
    var idx = this.games.indexOf(oldGame);
    if(idx !== -1){
      this.games[idx] = game;
    }
    return true;
  }

  deleteGame(game: Game):boolean{
    this.games = this.games.filter(e => e !== game);
    return true;
  }

  games: Game[] = [
    { gameId: "d3312c0d-a545-4ea9-9a43-fad7dba72f28", description: "Sieggraben - Steinberg", home: false, place: "Sportplatz Sieggraben", starting: "15:00", gameDay: new Date(2017,8,13), teamId: "8ab1ebd7-e15a-46e8-83a8-a816933b63c0" },
    { gameId: "22a06927-c009-465b-9ad5-e7c89f4b11fe", description: "Steinberg - Frankenau", home: true, place: "Sportplatz Steinberg", starting: "15:00", gameDay: new Date(2017,8,20), teamId: "8ab1ebd7-e15a-46e8-83a8-a816933b63c0" },
    { gameId: "6ba67b4d-ef19-4442-8878-6a84b4f9d49a", description: "Kaisersdorf - Steinberg", home: false, place: "Sportplatz Kaisersdorf", starting: "17:30", gameDay: new Date(2017,8,25), teamId: "8ab1ebd7-e15a-46e8-83a8-a816933b63c0" },
    { gameId: "c3029bbf-a73c-4cfb-9e59-44e9fdc30b83", description: "Steinberg - Neutal", home: true, place: "Sportplatz Steinberg", starting: "14:30", gameDay: new Date(2017,9,2), teamId: "8ab1ebd7-e15a-46e8-83a8-a816933b63c0" }
  ];
}
