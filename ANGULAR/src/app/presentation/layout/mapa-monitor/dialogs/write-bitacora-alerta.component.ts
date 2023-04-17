import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AlertasEntity } from '../../../../core/alertas/domain/alertas.entity';

@Component({
  selector: 'dialog-write-bitacora-alerta',
  templateUrl: './write-bitacora-alerta.component.html',
  styleUrls: ['./write-bitacora-alerta.component.scss'],
})
export class WriteAlertaBitacoraComponent {

  public bitacora = "";
  public error: string = "";

  constructor(
    public dialogRef: MatDialogRef<WriteAlertaBitacoraComponent>,
    @Inject(MAT_DIALOG_DATA) public data: AlertasEntity) {
  }

  public onCancel(): void {
    this.dialogRef.close(null);
  }
  public onAceptar(): void {
    this.data.bitacora = this.bitacora;
    if (this.data.bitacora === undefined || this.data.bitacora == null || this.data.bitacora.trim().length < 12) {
      this.error = "La bitacora tiene que tener un minimo de 12 caracteres";
      return;
    }
    this.dialogRef.close(this.data);
  }
}
