import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material';

@Component({
  selector: 'app-player-list',
  templateUrl: './player-list.component.html',
  styleUrls: ['./player-list.component.scss']
})
export class PlayerListComponent implements OnInit {

  displayedColumns = ['firstName', 'lastName', 'sqads'];

  constructor(public dialog: MatDialog) { }

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
