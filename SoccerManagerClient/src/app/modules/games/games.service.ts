import { Injectable } from '@angular/core';
import { Game } from './game';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class GamesService {

  constructor(private httpClient: HttpClient) { }

  getGames() {
    return Observable.create(obs => {
      obs.next(this.games);
      obs.complete();
    });
  }




  games: Game[] = [
    { description: "Sieggraben - Steinberg", home: false, place: "Sportplatz Sieggraben", starting: "15:00", gameDay: new Date("13.08.2017") },
    { description: "Steinberg - Frankenau", home: true, place: "Sportplatz Steinberg", starting: "15:00", gameDay: new Date("20.08.2017") },
    { description: "Kaisersdorf - Steinberg", home: false, place: "Sportplatz Kaisersdorf", starting: "17:30", gameDay: new Date("25.08.2017") },
    { description: "Steinberg - Neutal", home: true, place: "Sportplatz Steinberg", starting: "14:30", gameDay: new Date("02.09.2017") }
  ];
}
