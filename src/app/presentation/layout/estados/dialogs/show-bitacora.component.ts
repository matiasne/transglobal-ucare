import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'dialog-show-bitacora',
  templateUrl: './show-bitacora.component.html',
  styleUrls: ['./show-bitacora.component.scss'],
})
export class ShowBitacoraComponent {

  constructor(
    public dialogRef: MatDialogRef<ShowBitacoraComponent>,
    @Inject(MAT_DIALOG_DATA) public data: string) {
  }

  public onCancel(): void {
    this.dialogRef.close(null);
  }
}
