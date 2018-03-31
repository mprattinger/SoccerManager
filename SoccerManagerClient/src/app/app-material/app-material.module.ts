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
  MatDialogModule,
  MatDatepickerModule,
  MatNativeDateModule} from '@angular/material';

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
    MatTabsModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatDialogModule,
    MatButtonModule
  ],
  declarations: []
})
export class AppMaterialModule { }
