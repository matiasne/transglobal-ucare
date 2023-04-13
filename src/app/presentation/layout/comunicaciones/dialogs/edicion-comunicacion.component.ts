import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import * as moment from 'moment';
import { ComunicacionEntity } from '../../../../core/comunicaciones/domain/comunicacion.entity';

@Component({
  selector: 'dialog-edicion-comunicacion',
  templateUrl: './edicion-comunicacion.component.html',
  styleUrls: ['./edicion-comunicacion.component.scss'],
})
export class EdicionComunicacionComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<EdicionComunicacionComponent>,
    public formBuilder: FormBuilder,
    @Inject(MAT_DIALOG_DATA) public data: { entity: ComunicacionEntity, totalAfiliados: number, onAction: Function }) {
  }

  public enviarAhora: boolean = true;
  public fecha: Date = new Date();
  public hora: Date = new Date();
  public titulo: string = "";
  public mensaje: string = "";
  public estado: boolean = false;

  ngOnInit(): void {
    this.titulo = this.data.entity.titulo;
    this.mensaje = this.data.entity.mensaje;
    this.fecha = this.data.entity.fechaEnvio ?? new Date();
    this.hora = this.data.entity.fechaEnvio ?? new Date();
    this.enviarAhora = (this.data.entity.fechaEnvio ?? new Date(0)).getTime() < new Date().getTime();
    this.estado = this.data.entity.estado == "A";
  }

  public onChangeEnvio() {

  }

  public onClickAction() {
    this.dialogRef.close(null);
    this.data.entity.titulo = this.titulo;
    this.data.entity.mensaje = this.mensaje;
    let fecha = moment(this.fecha).toDate();
    let hora = moment(this.hora).toDate();
    this.data.entity.fechaEnvio = this.enviarAhora ? null : new Date(fecha.getFullYear(), fecha.getMonth(), fecha.getDate(), hora.getHours(), hora.getMinutes());
    this.data.entity.estado = this.estado ? "A" : "D";
    this.data.onAction(this.data.entity);
  }

  public onCancel(): void {
    this.dialogRef.close(null);
  }


}
