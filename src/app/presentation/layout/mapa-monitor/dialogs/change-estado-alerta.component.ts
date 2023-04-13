import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AlertasEntity } from '../../../../core/alertas/domain/alertas.entity';

@Component({
  selector: 'dialog-change-estado-alerta',
  templateUrl: './change-estado-alerta.component.html',
  styleUrls: ['./change-estado-alerta.component.scss'],
})
export class ChangeEstadoAlertaComponent {

  constructor(
    public dialogRef: MatDialogRef<ChangeEstadoAlertaComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { estado: string, alerta: AlertasEntity }) {
  }

  public onCancel(): void {
    this.dialogRef.close(null);
  }
  public onAceptar(): void {
    this.dialogRef.close(this.data);
  }
}
