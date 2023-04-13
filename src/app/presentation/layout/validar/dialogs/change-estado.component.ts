import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { lastValueFrom } from 'rxjs';
import { AfiliadoApp } from '../../../../core/afiliados/application/afiliado.app';
import { AfiliadoEntity } from '../../../../core/afiliados/domain/afiliado.entity';
import { SelectItem } from '../../../../shared/controller/controller.service';

@Component({
  selector: 'dialog-change-estado',
  templateUrl: './change-estado.component.html',
  styleUrls: ['./change-estado.component.scss'],
})
export class ChangeEstadoComponent {
  public estados: Array<SelectItem> = [{ id: "A", label: "Activo" }, { id: "D", label: "Inactivo" }, { id: "R", label: "Provisorio" }];
  public formEdit: FormGroup = this.formBuilder.group(
    {
      id: [],
      estado: ['T'],

    });
  constructor(
    public formBuilder: FormBuilder,
    public dialogRef: MatDialogRef<ChangeEstadoComponent>,
    public appCrud: AfiliadoApp,
    @Inject(MAT_DIALOG_DATA) public data: AfiliadoEntity) {

    this.formEdit.reset();
    this.formEdit.controls["id"].setValue(data.id);
    this.formEdit.controls["estado"].setValue(data.estado);
  }


  public onCancel(): void {
    this.dialogRef.close(null);
  }

  public async onCambiar() {

    var result = await lastValueFrom(this.appCrud.ChangeEstado(this.formEdit.value));
    this.dialogRef.close(result);
  }
}
