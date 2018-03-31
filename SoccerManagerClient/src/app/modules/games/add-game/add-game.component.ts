import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators, AbstractControl, ValidatorFn } from '@angular/forms';


@Component({
  selector: 'app-add-game',
  templateUrl: './add-game.component.html',
  styleUrls: ['./add-game.component.scss']
})
export class AddGameComponent implements OnInit {

  addGameForm: FormGroup;


  constructor() { }

  ngOnInit() {
    this.addGameForm = new FormGroup({
      gameDay: new FormControl('', Validators.required),
      starting: new FormControl('', [Validators.required, this.checkTime])
    });
  }

  checkTime(): ValidatorFn{
    return (control: AbstractControl): {[key: string]: any} => {
      return null;
    };
  }
}
