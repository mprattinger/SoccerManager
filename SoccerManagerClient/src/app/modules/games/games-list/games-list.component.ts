import { Component, OnInit } from '@angular/core';
import { GamesService } from '../games.service';
import { Game } from '../game';
import { MatTableDataSource, MatDialog } from '@angular/material';
import { AddGameComponent } from '../add-game/add-game.component';

@Component({
  selector: 'app-games-list',
  templateUrl: './games-list.component.html',
  styleUrls: ['./games-list.component.scss']
})
export class GamesListComponent implements OnInit {
  displayedColumns = ['starting', 'description', 'place', 'actions'];

  games: MatTableDataSource<Game>;

  constructor(private gamesService: GamesService,
  public dialog: MatDialog) { }

  ngOnInit() {
    this.loadData();
  }

  applyFilter(filterString: string){
    if(filterString != "" && filterString.length <= 3) return;

    filterString = filterString.trim();
    filterString = filterString.toLowerCase();

    this.games.filter = filterString;
  }

  addNew(){
    var diag = this.dialog.open(AddGameComponent);

    diag.afterClosed().subscribe(res => {
      if(res === 1){
        this.loadData();
      }
    });
  }

  loadData(){
    this.gamesService.getGames().subscribe(
      (games)=>{
        this.games = new MatTableDataSource(games);
      }
    );
  }
}
