import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';

@Component({
  selector: 'app-yes-no-dialog',
  templateUrl: './yes-no-dialog.component.html',
  styleUrls: ['./yes-no-dialog.component.scss']
})
export class YesNoDialogComponent implements OnInit {

  constructor(@Inject(MAT_DIALOG_DATA) public message: string,
              private dialogRef: MatDialogRef<YesNoDialogComponent>) { }

  ngOnInit() {
  }

  close(){
    this.dialogRef.close();
  }
}
