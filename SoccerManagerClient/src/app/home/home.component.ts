import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit, AfterViewInit {

  @ViewChild('gamesTabGroup') gamesTabGroup;

  constructor() { }

  ngOnInit() {
  }

  ngAfterViewInit(){
    this.gamesTabGroup.selectedIndex = 1;
  }
}
