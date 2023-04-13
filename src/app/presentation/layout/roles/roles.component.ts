import { ChangeDetectorRef, Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { ColDef, ColGroupDef, ICellRendererParams } from 'ag-grid-community';
import { lastValueFrom } from 'rxjs';
import { RolesApp } from '../../../core/roles/application/roles.app';
import { RolesEntity } from '../../../core/roles/domain/roles.entity';
import { AlertComponent } from '../../../shared/alert/alert.component';
import { Constants } from '../../../shared/Constants';
import { CrudControllerComponent, CrudMode, SelectItem } from '../../../shared/controller/controller.service';
import { ActionButtonData } from '../../../shared/grid/action-buttons.component';
import { ClickedEvent } from '../../../shared/grid/grid.component';
import { ModelsService } from '../../../shared/guard/model';
import { PagingFilter } from '../../../shared/rest/rest.service';
import { ChageUsersComponent } from './dialogs/cange-users.component';
import { ShowUsersComponent } from './dialogs/show-users.component';

@Component({
  selector: 'app-roles',
  templateUrl: './roles.component.html',
  styleUrls: ['./roles.component.scss']
})
export class RolesComponent extends CrudControllerComponent<RolesEntity> {

  public codigoPostal: string = "";
  public rolesAdmin: Array<SelectItem> = [{ id: "M", label: "Monitor" }, { id: "V", label: "Verificador" }];

  public roles: Array<SelectItem> = [{ id: "G", label: "Gerente" }, { id: "A", label: "Administrador" }, { id: "M", label: "Monitor" }, { id: "V", label: "Verificador" }];

  public subordinadosTodos: Array<SelectItem> = [{ id: "false", label: "Todos" }, { id: "true", label: "Propios" }];

  public usuariosNombre: Array<RolesEntity> = [];

  public codigosPostales: Array<string> | undefined | null = [];

  public codigosPostalesEdicion: Array<object> = [];

  public override formSearch: FormGroup | undefined = this.formBuilder.group(
    {
      estado: ['T'],
      subordinados: ["false"]
    });

  public override formEdit: FormGroup | undefined = this.formBuilder.group(
    {
      id: [],
      usuarioNombre: ["", Validators.required],
      email: ["", [Validators.required, Validators.email]],
      password: [""],
      estado: ["A", Validators.required],
      codigoPostal: [[]],
      rol: []
    });

  public override columnDefs: Array<ColDef | ColGroupDef> | undefined = [
    {
      headerName: "Nombre",
      field: "usuarioNombre",
      sortable: true,
      sort: 'asc',
    },
    {
      headerName: "E-Mail",
      field: "email",
      sortable: true,
    },
    {
      headerName: "Jefe",
      field: "usuarioId",
      sortable: true,
      cellRenderer: (params: ICellRendererParams) => {
        return this.usuariosNombre.find((item) => item.id == params.value)?.usuarioNombre ?? "";
      },
    },
    {
      headerName: "Rol",
      field: "rol",
      cellRenderer: (params: ICellRendererParams) => {
        return this.roles.find((item) => item.id == params.value)?.label ?? "";
      },
      sortable: false,
    },
    {
      headerName: "Estado",
      field: "estado",
      width: 60,
      cellRenderer: (params: ICellRendererParams) => {
        return this.getEstado(params.value);
      },
    },
    this.ColumnAction([
      { icon: "supervised_user_circle", title: "Subordinados", action: "usuarios", isShow: (item, param) => { return this.modelService.user!.rol !== "A" && this.modelService.user!.id === param.data.usuarioId } },
      { icon: "check_circle", title: "Activar", action: "activar", isShow: (item, param) => { return this.modelService.user!.id === param.data.usuarioId && param.data.estado != "A" } },
      { icon: "do_not_disturb_on", title: "Desactivar", action: "desactivar", isShow: (item, param) => { return this.modelService.user!.id === param.data.usuarioId && param.data.estado == "A" } },
      { icon: "edit", title: "Editar", action: "edit", isShow: (item, param) => { return this.modelService.user!.id === param.data.usuarioId } },
      { icon: "delete", title: "Borrar", action: "delete", isShow: (item, param) => { return this.modelService.user!.id === param.data.usuarioId } },
      { icon: "visibility", title: "Ver", action: "ver", isShow: (item, param) => { return this.modelService.user!.id !== param.data.usuarioId } }
    ] as Array<ActionButtonData>, 150)
  ];

  public userToChange: RolesEntity | undefined;

  public getId(data: RolesEntity) {
    return data.id;
  }

  public getLabel(data: RolesEntity) {
    return data.usuarioNombre ?? data.email ?? "";
  }

  public override onSearchBefore(): boolean {

    if (this.formSearch !== undefined) {

      const buscar = this.formSearch.value;

      if (buscar.estado !== undefined)
        this.page.filter.push({ property: 'Estado', condition: "=|T", value: buscar.estado } as PagingFilter);
      if (buscar.subordinados === "true")
        this.page.filter.push({ property: 'UsuarioId', condition: "=", value: this.modelService.user?.id } as PagingFilter);
    }

    return true;
  }

  public override onGridRowClicked(action: ClickedEvent) {
    if (action !== undefined && action.action > "") {
      switch (action.action) {
        case "ver":
        case "edit": {
          this.onEdit(action.data as RolesEntity);
          break;
        }
        case "delete": {
          this.onDeleteUser(action.data as RolesEntity);
          break;
        }
        case "usuarios": {
          this.onOpenUsuarios(action.data as RolesEntity);
          break;
        }
        case "desactivar": {
          this.onDesactivarUser(action.data as RolesEntity);
          break;
        }
        case "activar": {
          this.onActivarUser(action.data as RolesEntity);
          break;
        }
        default: {
          alert(action.action);
          break;
        }
      }
    }
  }

  public override onEdit(data: RolesEntity) {
    super.onEdit(data);
    if (this.item?.usuarioId === this.modelService.user?.id || this.item?.usuarioId === undefined || this.item?.usuarioId === null) {
      if (this.codigosPostales !== undefined && this.codigosPostales !== null && this.codigosPostales.length !== 0)
        this.codigosPostalesEdicion = this.codigosPostales! as any;
      else
        this.codigosPostalesEdicion = (this.item?.codigoPostal ?? [] as any);
    }
    else {
      this.codigosPostalesEdicion = this.item.codigoPostal as any;
    }
  }

  public onActivarUser(data: RolesEntity) {
    if (this.onDeleteBefore(data)) {
      const dialog = this.dialog.open(AlertComponent, { data: { titulo: "Activar", label: "¿Está seguro de querer activar este usuario?", type: "warning" }, panelClass: ['mo-alert'], autoFocus: false });
      dialog.afterClosed().subscribe(async modalAction => {
        if (modalAction) {
          this.modelService.showHourglass = true;
          data!.estado = "A";
          let resul = (await lastValueFrom(this.appCrud.Save(data!)));
          if (resul.isError) {
            this.openDialogError(resul.data as any);
          }
          else {
            this.openDialogOk("Operación realizada con éxito");
          }
          this.modelService.showHourglass = false;
        }
      })
    }
  }

  public onDesactivar(data: RolesEntity) {
    if (this.onDeleteBefore(data)) {
      const dialog = this.dialog.open(AlertComponent, { data: { titulo: "Desactivar", label: "Está seguro de querer desactivar este usuario", type: "warning" }, panelClass: ['mo-alert'], autoFocus: false });
      dialog.afterClosed().subscribe(async modalAction => {
        if (modalAction) {
          this.modelService.showHourglass = true;
          data!.estado = "D";
          let resul = (await lastValueFrom(this.appCrud.Save(data!)));
          if (resul.isError) {
            this.openDialogError(resul.data as any);
          }
          else {
            this.openDialogOk("Operación realizada con éxito");
          }
          this.modelService.showHourglass = false
        }
      })
    }
  }

  public onDesactivarUser(data: RolesEntity) {
    this.userToChange = data;
    if (this.userToChange.rol == "M" || this.userToChange.rol == "V") {
      this.onDesactivar(data);
    }
    else {
      const modal = this.dialog.open(ChageUsersComponent, {
        data: {
          icon: "do_not_disturb_on",
          labeInput: "Seleccionar usuario",
          header1:
            this.modelService.user?.rol === "P" ? "Suspenderás un usuario gerente" :
              this.modelService.user?.rol === "G" ? "Suspenderás un usuario administrador" :
                "",
          header2:
            this.modelService.user?.rol === "P" ? "Para suspender un usuario gerente debes asignar otro a sus Subordinados" :
              this.modelService.user?.rol === "G" ? "Para suspender un usuario administrador debes asignar sus subordinados a otro administrador." :
                "",
          buttonAction: "Confirmar y suspender",
          entity: data, onAction: this.onDesactivarConfirm.bind(this)
        }, maxWidth: "800px"
      });
      modal.afterClosed().subscribe(() => {
        this.onSearch();
      });
    }
  }

  public onDesactivarConfirm(data: string) {
    const dialog = this.dialog.open(AlertComponent, { data: { titulo: "Desactivar el Usuario", label: "Se está por desactivar el usuario. ¿Está seguro de realizar esta operación?", type: "warning" }, panelClass: ['mo-alert'], autoFocus: false });
    dialog.afterClosed().subscribe(async (result) => {
      if (result) {
        this.modelService.showHourglass = true;
        var resul = (await lastValueFrom(this.appCrud.GetReplaceTo(this.getId(this.userToChange!) ?? "", data ?? "")));
        if (resul.isError) {
          this.openDialogError(resul.data);
        }
        else {
          this.userToChange!.estado = "D";
          resul = (await lastValueFrom(this.appCrud.Save(this.userToChange!)));
          if (resul.isError) {
            this.openDialogError(resul.data);
          }
          else {
            this.openDialogOk("Operación realizada con éxito");
          }
        }
        this.modelService.showHourglass = false;
        this.onCancel(true);
      }
    })
  }

  public onDeleteUser(data: RolesEntity) {
    this.userToChange = data;
    if (this.userToChange.rol == "M" || this.userToChange.rol == "V") {
      this.onDelete(data);
    }
    else {
      const modal = this.dialog.open(ChageUsersComponent, {
        data: {
          icon: "delete",
          labeInput: "Seleccionar usuario",
          header1:
            this.modelService.user?.rol === "P" ? "Eliminarás un usuario gerente" :
              this.modelService.user?.rol === "G" ? "Eliminarás un usuario administrador" :
                "",
          header2:
            this.modelService.user?.rol === "P" ? "Para eliminar un usuario gerente debes asignar otro a sus Subordinados" :
              this.modelService.user?.rol === "G" ? "Para eliminar un usuario administrador debes asignar sus subordinados a otro administrador." :
                "",
          buttonAction: "Confirmar y eliminar",
          entity: data, onAction: this.onDeleteConfirm.bind(this)
        }, maxWidth: "800px"
      });
      modal.afterClosed().subscribe(() => {
        this.onSearch();
      });
    }
  }

  public onDeleteConfirm(data: string) {
    const dialog = this.dialog.open(AlertComponent, { data: { titulo: "Borrar el usuario", label: "Se está por borrar el usuario. ¿Está seguro de realizar esta operación?", type: "warning" }, panelClass: ['mo-alert'], autoFocus: false });
    dialog.afterClosed().subscribe(async (result) => {
      if (result) {
        this.modelService.showHourglass = true;
        var resul = (await lastValueFrom(this.appCrud.GetReplaceTo(this.getId(this.userToChange!) ?? "", data ?? "")));
        if (resul.isError) {
          this.openDialogError(resul.data);
        }
        else {
          resul = (await lastValueFrom(this.appCrud.Delete(this.getId(this.userToChange!) ?? "")));
          if (resul.isError) {
            this.openDialogError(resul.data);
          }
          else {
            this.openDialogOk("Operación realizada con éxito");
          }
        }
        this.modelService.showHourglass = false;
        this.onCancel(true);
      }
    })
  }

  public onOpenUsuarios(data: RolesEntity) {
    const modal = this.dialog.open(ShowUsersComponent, {
      data: {
        icon: "supervised_user_circle",
        header1: "Subordinados",
        header2:
          this.modelService.user?.rol === "P" ? "Estos son los usuarios Subordinados que responden a este gerente" :
            this.modelService.user?.rol === "G" ? "Estos son los usuarios subordinados que responden a este administrador" :
              "",
        buttonAction: "Reasignar a otro superior",
        entity: data, onAction: this.onChangeUser.bind(this)
      }, maxWidth: "800px"
    });
    modal.afterClosed().subscribe(() => {
      this.onSearch();
    });
  }

  public onChangeUser(data: RolesEntity) {
    this.userToChange = data;
    const modal = this.dialog.open(ChageUsersComponent, {
      data: {
        icon: "supervised_user_circle",
        labeInput: "Seleccionar usuario",
        header1: "Subordinados",
        header2:
          this.modelService.user?.rol === "P" ? "Estos son los usuarios Subordinados que responden a este propietario" :
            this.modelService.user?.rol === "G" ? "Estos son los usuarios Subordinados que responden a este gerente" :
              "",
        buttonAction: "Confirmar",
        entity: data, onAction: this.onChangeUserConfirm.bind(this)
      }, maxWidth: "800px"
    });
    modal.afterClosed().subscribe(() => {
      this.onSearch();
    });
  }

  public onChangeUserConfirm(data: string) {
    const dialog = this.dialog.open(AlertComponent, { data: { titulo: "Cambiar el Usuario", label: "Se está por cambiar el usuario", type: "warning" }, panelClass: ['mo-alert'], autoFocus: false });
    dialog.afterClosed().subscribe(async (result) => {
      if (result) {
        this.modelService.showHourglass = true;
        let resul = (await lastValueFrom(this.appCrud.GetReplaceTo(this.getId(this.userToChange!) ?? "", data ?? "")));
        if (resul.isError) {
          this.openDialogError(resul.data);
        }
        else {
          this.openDialogOk("Operación realizada con éxito");
        }
        this.modelService.showHourglass = false;
        this.onCancel(true);
      }
    })
  }

  public codePostalColumnDefs: Array<ColDef | ColGroupDef> = [
    this.ColumnAction([
      {
        icon: "check_box", title: "Deseleccionar", action: "deseleccionar", isShow: (item, param) => {
          return (this.item?.usuarioId === this.modelService.user?.id || this.item?.usuarioId === undefined || this.item?.usuarioId === null) &&
            (this.codigosPostales !== undefined && this.codigosPostales !== null && this.codigosPostales.length !== 0) &&
            (this.formEdit?.value.codigoPostal.indexOf(param.data) > -1);
        }
      },
      {
        icon: "check_box_outline_blank", title: "Seleccionar", action: "seleccionar", isShow: (item, param) => {
          return (this.item?.usuarioId === this.modelService.user?.id || this.item?.usuarioId === undefined || this.item?.usuarioId === null) &&
            (this.codigosPostales !== undefined && this.codigosPostales !== null && this.codigosPostales.length !== 0) &&
            (this.formEdit?.value.codigoPostal.indexOf(param.data) == -1);
        }
      },
      {
        icon: "delete", title: "Borrar", action: "delete", isShow: (item, param) => {
          return (this.item?.usuarioId === this.modelService.user?.id || this.item?.usuarioId === undefined || this.item?.usuarioId === null) &&
            !(this.codigosPostales !== undefined && this.codigosPostales !== null && this.codigosPostales.length !== 0);
        }
      },
    ] as Array<ActionButtonData>),
    {
      headerName: "Código postal",
      cellRenderer: (params: ICellRendererParams) => {
        return params.data;
      },
    },
  ];

  public onAddCodePostal() {
    if (this.codigoPostal !== undefined && this.codigoPostal != null && this.codigoPostal.trim() !== "") {
      const code = (this.formEdit?.value?.codigoPostal ?? []) as Array<string>;
      if (code.find((item) => {
        return item === this.codigoPostal;
      }) === undefined) {
        code.push(this.codigoPostal);
        this.codigoPostal = "";
        this.formEdit?.controls["codigoPostal"].setValue(code);
        this.codigosPostalesEdicion = this.formEdit?.controls["codigoPostal"].value;
      }
    }
  }

  public onSelectAll() {
    this.formEdit?.controls["codigoPostal"].setValue(this.codigosPostalesEdicion.filter(() => true));
  }

  public onUnSelectAll() {
    this.formEdit?.controls["codigoPostal"].setValue([]);
  }

  public onCodePostalClicked(action: ClickedEvent) {
    if (action !== undefined && action.action > "") {
      switch (action.action) {
        case "seleccionar": {
          const code = (this.formEdit?.value?.codigoPostal ?? []) as Array<string>;
          code.push(action.data);
          this.formEdit?.controls["codigoPostal"].setValue(code);
          break;
        }
        case "deseleccionar":
          {
            const code = (this.formEdit?.value?.codigoPostal ?? []) as Array<string>;
            this.formEdit?.controls["codigoPostal"].setValue(code.filter((item) => { return item !== action.data }));
            break;
          }
        case "delete": {
          const code = (this.formEdit?.value?.codigoPostal ?? []) as Array<string>;
          this.formEdit?.controls["codigoPostal"].setValue(code.filter((item) => { return item !== action.data }));
          this.codigosPostalesEdicion = this.formEdit?.controls["codigoPostal"].value;
          break;
        }
      }
    }
  }

  constructor(
    public override router: Router,
    public override activatedRoute: ActivatedRoute,
    public override ref: ChangeDetectorRef,
    public override formBuilder: FormBuilder,
    public override dialog: MatDialog,
    public override modelService: ModelsService,
    public override appCrud: RolesApp,
  ) {
    super(router, activatedRoute, ref, formBuilder, dialog, modelService, Constants.LAYOUT_ROLES, appCrud, RolesEntity, CrudMode.Search);

    appCrud.GetAllUserManager().subscribe((result) => {
      if (!result.isError)
        this.usuariosNombre = result.data as Array<RolesEntity>;
    });

    appCrud.GetCodigosPostales().subscribe((result) => {
      if (!result.isError)
        this.codigosPostales = result.data as string[];
    });
  }

}
