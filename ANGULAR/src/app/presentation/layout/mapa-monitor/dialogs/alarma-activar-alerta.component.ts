import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AlertasEntity } from '../../../../core/alertas/domain/alertas.entity';

@Component({
  selector: 'dialog-alarma-activar-alerta',
  templateUrl: './alarma-activar-alerta.component.html',
  styleUrls: ['./alarma-activar-alerta.component.scss'],
})
export class AlarmaActivarAlertaComponent {

  constructor(
    public dialogRef: MatDialogRef<AlarmaActivarAlertaComponent>,
    @Inject(MAT_DIALOG_DATA) public data: AlertasEntity) {
  }

  public onCancel(): void {
    this.dialogRef.close(null);
  }
  public onAceptar(): void {
    this.dialogRef.close(this.data);
  }
}
