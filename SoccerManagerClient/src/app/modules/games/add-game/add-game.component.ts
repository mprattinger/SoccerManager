import { Router } from '@angular/router';
import { GamesService } from './../games.service';
import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormControl, Validators, AbstractControl, ValidatorFn } from '@angular/forms';
import { Game } from '../game';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Location } from '@angular/common';


@Component({
  selector: 'app-add-game',
  templateUrl: './add-game.component.html',
  styleUrls: ['./add-game.component.scss']
})
export class AddGameComponent implements OnInit {

  addGameForm: FormGroup;
  changeMode = false;
  okButtonText = "Anlegen";

  constructor(private gamesService: GamesService,
    private router: Router,
    private dialogRef: MatDialogRef<AddGameComponent>,
    private location: Location,
    @Inject(MAT_DIALOG_DATA) public gameToChange: Game) { }

  ngOnInit() {
    let game = new Game();
    if (this.gameToChange) {
      game = this.gameToChange;
      this.changeMode = true;
      this.okButtonText = "Speichern";
    }

    this.addGameForm = new FormGroup({
      gameDay: new FormControl(game.gameDay, Validators.required),
      starting: new FormControl(game.starting, [Validators.required, Validators.pattern("^([0-9]|0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$")]),
      description: new FormControl(game.description, [Validators.required]),
      place: new FormControl(game.place, [Validators.required]),
      home: new FormControl(game.home)
    });
  }

  onFormSubmit() {

  }

  add(game: Game) {
    if (game) {
      if (!this.changeMode) {
        if (this.gamesService.addGame(game)) {
          this.dialogRef.close(1);
        }
      } else {
        if (this.gamesService.changeGame(this.gameToChange, game)) {
          this.dialogRef.close(1);
        }
      }
    }
  }

  close() {
    this.dialogRef.close();
  }
}