import { YesNoDialogComponent } from './../../../common/yes-no-dialog/yes-no-dialog.component';
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
  displayedColumns = ['gameDay', 'starting', 'description', 'place', 'actions'];

  games: MatTableDataSource<Game>;
  teams= [{ teamId: "8ab1ebd7-e15a-46e8-83a8-a816933b63c0", teamName: "Reserve"}]
  selectedTeam: string;

  constructor(private gamesService: GamesService,
  public dialog: MatDialog) { }

  ngOnInit() {
    this.selectedTeam = "8ab1ebd7-e15a-46e8-83a8-a816933b63c0";
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

  change(game){
    var diag = this.dialog.open(AddGameComponent, { data: game});

    diag.afterClosed().subscribe(res => {
      if(res === 1){
        this.loadData();
      }
    });
  }

  delete(game){
    var diag = this.dialog.open(YesNoDialogComponent, { data: "Wollen Sie das Spiel wirklich löschen?"});

    diag.afterClosed().subscribe(res => {
      if(res === 1){
        if(this.gamesService.deleteGame(game)) this.loadData();
      }
    });
  }

  loadData(){
    this.gamesService.getGames(this.selectedTeam).subscribe(
      (games)=>{
        this.games = new MatTableDataSource(games);
      }
    );
  }
}
