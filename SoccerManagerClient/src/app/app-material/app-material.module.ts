import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MatInputModule,
  MatButtonModule,
  MatSidenavModule,
  MatToolbarModule,
  MatIconModule,
  MatListModule,
  MatTabsModule,
  MatTableModule,
  MatSelectModule,
  MatDialogModule,
  MatDatepickerModule,
  MatNativeDateModule,
  MatCheckboxModule} from '@angular/material';

@NgModule({
  imports: [
    CommonModule
  ],
  exports:[
    MatInputModule,
    MatSidenavModule,
    MatTableModule,
    MatIconModule,
    MatToolbarModule,
    MatListModule,
    MatCheckboxModule,
    MatSelectModule,
    MatTabsModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatDialogModule,
    MatButtonModule
  ],
  declarations: []
})
export class AppMaterialModule { }
