import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ColDef, ColGroupDef, ICellRendererParams } from 'ag-grid-community';
import { lastValueFrom } from 'rxjs';
import { RolesApp } from '../../../../core/roles/application/roles.app';
import { RolesEntity } from '../../../../core/roles/domain/roles.entity';
import { SelectItem } from '../../../../shared/controller/controller.service';

@Component({
  selector: 'dialog-show-users',
  templateUrl: './show-users.component.html',
  styleUrls: ['./show-users.component.scss'],
})
export class ShowUsersComponent implements OnInit {

  public datagrid: Array<RolesEntity> = [];
  public roles: Array<SelectItem> = [{ id: "G", label: "Gerente" }, { id: "A", label: "Administrador" }, { id: "M", label: "Monitor" }, { id: "V", label: "Verificador" }];
  public estados: Array<SelectItem> = [{ id: "A", label: "Activo" }, { id: "D", label: "Inactivo" }];

  constructor(
    public dialogRef: MatDialogRef<ShowUsersComponent>,
    public formBuilder: FormBuilder,
    public appCrud: RolesApp,
    @Inject(MAT_DIALOG_DATA) public data: { icon: string, header1: string, header2: string, buttonAction: string, entity: RolesEntity, onAction: Function }) {
  }

  public showLoadingOverlay = true;

  public columnDefs: Array<ColDef | ColGroupDef> = [
    {
      headerName: "Nombre",
      field: "usuarioNombre",
      sortable: true,
    },
    {
      headerName: "E-Mail",
      field: "email",
      sortable: true,
    },
    {
      headerName: "Rol",
      field: "rol",
      cellRenderer: (params: ICellRendererParams) => {
        return this.roles.find((item) => item.id == params.value)?.label ?? "";
      },
      sortable: true,
    },
    {
      headerName: "Estado",
      field: "estado",
      width: 70,
      minWidth:70,
      cellRenderer: (params: ICellRendererParams) => {
        return this.estados.find((item) => item.id == params.value)?.label ?? "";
      }
    }
  ];

  ngOnInit(): void {
    this.onLoadData();
  }

  public async onLoadData() {
    try {
      var resul = (await lastValueFrom(this.appCrud.GetUser(this.data.entity.id ?? "")));
      if (!resul.isError)
        this.datagrid = resul.data ?? [];
      else {

      }
    }
    catch (error) {

    }
    this.showLoadingOverlay = false;
  }

  public onClickAction() {
    this.dialogRef.close(null);
    this.data.onAction(this.data.entity);
  }

  public onCancel(): void {
    this.dialogRef.close(null);
  }


}
