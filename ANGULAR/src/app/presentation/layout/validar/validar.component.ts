import { ChangeDetectorRef, Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { ColDef, ColGroupDef, ICellRendererParams } from 'ag-grid-community';
import * as moment from 'moment';
import { AfiliadoApp } from '../../../core/afiliados/application/afiliado.app';
import { AfiliadoEntity } from '../../../core/afiliados/domain/afiliado.entity';
import { Constants } from '../../../shared/Constants';
import { CrudControllerComponent, CrudMode, SelectItem } from '../../../shared/controller/controller.service';
import { ClickedEvent } from '../../../shared/grid/grid.component';
import { ModelsService } from '../../../shared/guard/model';
import { PagingFilter } from '../../../shared/rest/rest.service';
import { ChangeEstadoComponent } from './dialogs/change-estado.component';

@Component({
  selector: 'app-validar',
  templateUrl: './validar.component.html',
  styleUrls: ['./validar.component.scss']
})
export class ValidarComponent extends CrudControllerComponent<AfiliadoEntity> {
  public IsNoAfiliado: boolean = false;
  public IsAfiliado: boolean = false;
  public IsActivos: boolean = false;
  public IsInactivos: boolean = false;
  public IsProvisorio: boolean = true;

  public Activo = "A";
  public Desactivo = "D";
  public Provisorio = "R";

  public estadosAfiliado: Array<SelectItem> = [{ id: "A", label: "Activo" }, { id: "D", label: "Inactivo" }, { id: "R", label: "Provisorio" }];
  public sexosAfiliado: Array<SelectItem> = [{ id: "M", label: "Masculino" }, { id: "F", label: "Femenino" }, { id: "O", label: "Otro" }];
  public sexosAfiliadoTodos: Array<SelectItem> = [{ id: "T", label: "Todos" }, { id: "M", label: "Masculino" }, { id: "F", label: "Femenino" }, { id: "O", label: "Otro" }];

  public getId(data: AfiliadoEntity) {
    return data.id;
  }
  public getLabel(data: AfiliadoEntity) {
    return data.usuarioNombre;
  }

  public override formSearch: FormGroup | undefined = this.formBuilder.group(
    {
      numeroIdentidad: [""],
      sexo: ["T"],
      celular: [""],
      email: [""],
    });

  public override formEdit: FormGroup | undefined = this.formBuilder.group(
    {
      id: [],
      usuarioNombre: ["", Validators.required],
      email: ["", Validators.required],
      numeroIdentidad: ["", Validators.required],
      fechaNacimiento: [""],
      sexo: ["", Validators.required],
      position: [this.formBuilder.group({
        lat: [0, Validators.required],
        lon: [0, Validators.required],
      })],
      celular: ["", Validators.required],
      codigoPais: ["", Validators.required],
      nosocomio: [""],
      patologias: [""],
      medicacion: [""],
      alergias: [""],
      contactos: [""],
      afiliacion: [""],
      estado: [""],
    });

  public formEditDireccion: FormGroup | undefined = this.formBuilder.group({
    calle: ["", Validators.required],
    nro: ["", Validators.required],
    piso: [""],
    barrio: [""],
    ciudad: ["", Validators.required],
    departamento: [""],
    codigoPostal: ["", Validators.required],
  });

  public override createFromEdit(data: AfiliadoEntity) {
    if (this.formEdit !== undefined) {
      this.formEdit.reset();
      const value = new this.ctor(data);
      this.item = value;
      this.setFormGroupByObject(this.formEdit, value);
    }
    if (this.formEditDireccion !== undefined) {
      this.formEditDireccion.reset();
      const value = new this.ctor(data);
      this.setFormGroupByObject(this.formEditDireccion, value.direccion);
    }
  }

  public override columnDefs: Array<ColDef | ColGroupDef> | undefined = [
    {
      headerName: "",
      field: "estado",
      width: 42,
      maxWidth: 42,
      minWidth: 42,
      cellRenderer: (params: ICellRendererParams) => {
        let border = "";
        if ((params.data?.afiliacion?.empresa ?? "-") === "-")
          border = '5px solid #6B0BF3';
        else
          border = '5px solid #2381EF';

        switch (params.value) {
          case "A":
            return `<div style="width: 14px;height: 14px; border: ${border};border-radius: 12px; background-color: #3BB54E;"></div>`;
          case "D":
            return `<div style="width: 14px;height: 14px; border: ${border};border-radius: 12px; background-color: #EF233C;"></div>`;
          case "R":
            return `<div style="width: 14px;height: 14px; border: ${border};border-radius: 12px; background-color: #EF9D23;"></div>`;
          default:
            return `<div style="width: 14px;height: 14px; border: ${border};border-radius: 12px; background-color: #999;"></div>`;
        }
      }
    },
    {
      headerName: "Nombre y Apellido",
      field: "usuarioNombre",
      width: 300,
      maxWidth: 300,
    },
    {
      headerName: "Teléfono",
      field: "celular",
      width: 160,
      maxWidth: 160,
      cellRenderer: (params: ICellRendererParams) => {
        return `<div data-action-type="copy" style="display: flex; flex-direction: row; flex-wrap: wrap; align-content: center; align-items: center; justify-content: space-between;">${params.value}<mat-icon data-action-type="copy" role="img" fontset="material-icons-outlined" mattooltipclass="mo-tooltip" class="mat-icon notranslate mat-tooltip-trigger material-icons-outlined mat-icon-no-color ng-star-inserted" aria-hidden="true" ng-reflect-font-set="material-icons-outlined" ng-reflect-tooltip-class="mo-tooltip" ng-reflect-message="Cambiar estado" data-mat-icon-type="font" data-mat-icon-namespace="material-icons-outlined" aria-describedby="cdk-describedby-message-2" cdk-describedby-host="0" style="cursor: pointer; color: #8D99AE;">content_copy</mat-icon></div>`;
      }
    },
    {
      headerName: "Domicilio",
      field: "direccion",
      cellRenderer: (params: ICellRendererParams) => {
        return `${params.data.direccion.departamento ?? ""} ${params.data.direccion.ciudad ?? ""} ${params.data.direccion.barrio ?? ""} ${params.data.direccion.calle ?? ""} ${params.data.direccion.nro ?? ""} Piso:${params.data.direccion.piso ?? ""} CP:${params.data.direccion.codigoPostal ?? ""} `;
      }
    },
    {
      headerName: "Nro. Iden.",
      field: "numeroIdentidad",
      width: 110,
      maxWidth: 110,
    },
    {
      headerName: "Email",
      field: "email",
      width: 200,
      maxWidth: 200,
    },
    {
      headerName: "Fecha",
      field: "creado.modificado",
      width: 130,
      maxWidth: 130,
      sortable: true,
      cellRenderer: (params: ICellRendererParams) => {
        return moment(params.value).format("DD/MM/YYYY HH:mm")
      }
    },
    {
      headerName: "Acciones",
      width: 90,
      maxWidth: 90,
      minWidth: 90,
      suppressMenu: true,
      sortable: false,
      suppressMovable: true,
      suppressAutoSize: true,
      resizable: false,
      pinned: "right",
      cellRenderer: "actionButtons",
      cellRendererParams: {
        buttons: [
          { icon: "change_circle", title: "Cambiar estado", action: "change" },
          { icon: "edit", title: "Editar", action: "edit" }]
      }
    }
  ];

  public override onGridRowClicked(action: ClickedEvent) {
    if (action !== undefined && action.action > "") {
      switch (action.action) {
        case "edit": {
          this.onEdit(action.data);
          break;
        }
        case "copy": {
          this.onCopyContent(action.data.celular, null);
          break;
        }
        case "change": {
          const diag = this.dialog.open(ChangeEstadoComponent, {
            data: action.data, maxWidth: "600px"
          });
          const subscrib = diag.afterClosed().subscribe(result => {
            subscrib.unsubscribe();
            if (!result.isError) {
              this.onSearch(true);
            }
            else {
              this.onSaveAfter(!result.isError, result);
            }
          });
          break;
        }
        default: {
          alert(action.action);
          break;
        }
      }
    }
  }

  public override onSearchBefore(): boolean {


    if (this.IsNoAfiliado || this.IsAfiliado || this.IsActivos || this.IsInactivos || this.IsProvisorio) {

      let arr = [];

      if (this.IsProvisorio) arr.push(this.Provisorio);
      if (this.IsActivos) arr.push(this.Activo);
      if (this.IsInactivos) arr.push(this.Desactivo);

      if (arr.length > 0)
        this.page.filter.push({
          property: 'Estado', condition: "x", value: arr
        } as PagingFilter);

      if (this.IsNoAfiliado != this.IsAfiliado) {
        if (this.IsNoAfiliado) {
          this.page.filter.push({
            property: 'Afiliacion.Empresa', condition: "=", value: "-"
          } as PagingFilter);
        }
        else {
          this.page.filter.push({
            property: 'Afiliacion.Empresa', condition: "!=", value: "-"
          } as PagingFilter);
        }
      }
    }

    if (this.formSearch !== undefined) {

      const buscar = this.formSearch.value;

      if (buscar.numeroIdentidad !== undefined && buscar.numeroIdentidad != "")
        this.page.filter.push({ property: 'NumeroIdentidad', condition: "=", value: buscar.numeroIdentidad } as PagingFilter);
      if (buscar.celular !== undefined && buscar.celular != "")
        this.page.filter.push({ property: 'Celular', condition: "=", value: buscar.celular } as PagingFilter);
      if (buscar.email !== undefined && buscar.email != "")
        this.page.filter.push({ property: 'Email', condition: "=", value: buscar.email } as PagingFilter);
      if (buscar.sexo !== undefined && buscar.sexo != "T")
        this.page.filter.push({ property: 'Sexo', condition: "=", value: buscar.sexo } as PagingFilter);
    }

    return true;
  }

  public onSelectNoAfiliad() {
    this.IsNoAfiliado = !this.IsNoAfiliado;
    this.onSearch(true);
  }

  public onSelectAfiliado() {
    this.IsAfiliado = !this.IsAfiliado;
    this.onSearch(true);
  }

  public onSelectProvisorio() {
    this.IsProvisorio = !this.IsProvisorio;
    this.IsActivos = false;
    this.IsInactivos = false;
    this.onSearch(true);
  }

  public onSelectActivos() {
    this.IsActivos = !this.IsActivos;
    this.IsProvisorio = false;
    this.IsInactivos = false;
    this.onSearch(true);
  }

  public onSelectInactivos() {
    this.IsInactivos = !this.IsInactivos;
    this.IsActivos = false;
    this.IsProvisorio = false;
    this.onSearch(true);
  }

  //COPIAMOS AL PORTAPAPELES UN TEXTO
  public onCopyContent(val: string, $event: Event | null) {
    $event?.stopPropagation();
    $event?.stopImmediatePropagation();
    if (!navigator.clipboard) {
      let selBox = document.createElement('textarea');
      selBox.style.position = 'fixed';
      selBox.style.left = '0';
      selBox.style.top = '0';
      selBox.style.opacity = '0';
      selBox.value = val;
      document.body.appendChild(selBox);
      selBox.focus();
      selBox.select();
      document.execCommand('copy');
      document.body.removeChild(selBox);
    } else {
      navigator.clipboard.writeText(val).then(
        function () {
        })
        .catch(
          function (err) {
            alert("err"); // error
          });
    }
  }

  public override onSaveBefore(data: AfiliadoEntity): boolean {
    data.direccion = this.formEditDireccion?.value;
    return true;
  }

  constructor(
    public override router: Router,
    public override activatedRoute: ActivatedRoute,
    public override ref: ChangeDetectorRef,
    public override formBuilder: FormBuilder,
    public override dialog: MatDialog,
    public override modelService: ModelsService,
    public override appCrud: AfiliadoApp,
  ) {
    super(router, activatedRoute, ref, formBuilder, dialog, modelService, Constants.LAYOUT_AFILIADO, appCrud, AfiliadoEntity, CrudMode.Search);
  }

}
