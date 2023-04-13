import { LabelType, Options } from '@angular-slider/ngx-slider';
import { ChangeDetectorRef, Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { ColDef, ColGroupDef, ICellRendererParams } from 'ag-grid-community';
import * as moment from 'moment';
import { lastValueFrom } from 'rxjs';
import { AfiliadoEntity } from '../../../core/afiliados/domain/afiliado.entity';
import { ComunicacionApp } from '../../../core/comunicaciones/application/comunicacion.app';
import { ComunicacionEntity } from '../../../core/comunicaciones/domain/comunicacion.entity';
import { Constants } from '../../../shared/Constants';
import { CrudControllerComponent, CrudMode, SelectItem } from '../../../shared/controller/controller.service';
import { ActionButtonData } from '../../../shared/grid/action-buttons.component';
import { ClickedEvent } from '../../../shared/grid/grid.component';
import { ModelsService } from '../../../shared/guard/model';
import { Page, PageGeneric, PagingFilter, ReturnData } from '../../../shared/rest/rest.service';
import { EdicionComunicacionComponent } from './dialogs/edicion-comunicacion.component';

@Component({
  selector: 'app-comunicacion',
  templateUrl: './comunicacion.component.html',
  styleUrls: ['./comunicacion.component.scss']
})
export class ComunicacionComponent extends CrudControllerComponent<ComunicacionEntity> {

  public Activo = "A";
  public Desactivo = "D";
  public Provisorio = "R";

  public estadosAfiliado: Array<SelectItem> = [{ id: "T", label: "Todos" }, { id: "A", label: "Activo" }, { id: "D", label: "Inactivo" }, { id: "R", label: "Provisorio" }];
  public estadosComunicacion: Array<SelectItem> = [{ id: "T", label: "Todos" }, { id: "A", label: "Activo" }, { id: "D", label: "Edición" }];
  public generosTodos: Array<SelectItem> = [{ id: "T", label: "Todos" }, { id: "M", label: "Masculino" }, { id: "F", label: "Femenino" }, { id: "O", label: "Otro" }];
  public enviadosTodos: Array<SelectItem> = [{ id: "T", label: "Todos" }, { id: "true", label: "Enviados" }, { id: "false", label: "No enviados" }];

  public edadMin: number = 0;
  public edadMax: number = 120;

  public optionsSlider: Options = {
    floor: 0,
    ceil: 120,
    step: 1,
    translate: (value: number, label: LabelType): string => {
      switch (label) {
        case LabelType.Low:
          return "<b>Edad min.:</b>" + value;
        case LabelType.High:
          return "<b>Edad max.:</b>" + value;
        default:
          return ''+ value;
      }
    }
  };

  public override formSearch: FormGroup | undefined = this.formBuilder.group(
    {
      estado: ['T'],
      enviado: ['false'],
      desde: [moment().subtract(15, "days").toDate()],
      hasta: [moment().add(30, "days").toDate()],
    });

  public formSearch2: FormGroup | undefined = this.formBuilder.group(
    {
      estado: ['T'],
      edad: [null],
      sexo: ['T'],
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
      headerName: "Título",
      field: "titulo",
    },
    {
      headerName: "Envío",
      field: "fechaEnvio",
      width: 90,
      cellRenderer: (params: ICellRendererParams) => {
        return params.value == null ? "Ahora" : moment(params.value).format("DD/MM/YYYY HH:mm");
      },
    },
    {
      headerName: "Detalle",
      field: "detalleEnvio",
    },
    {
      headerName: "Enviado",
      field: "enviado",
      width: 60,
      cellRenderer: (params: ICellRendererParams) => {
        return params.value ? "Si" : "No";
      },
    },
    {
      headerName: "Estado",
      field: "estado",
      width: 60,
      cellRenderer: (params: ICellRendererParams) => {
        return this.findElement(params.value, this.estadosComunicacion);
      },
    },
    this.ColumnAction([{ icon: "content_copy", title: "Duplicar", action: "clone" }, { icon: "edit", title: "Editar", action: "edit" }, { icon: "delete", title: "Borrar", action: "delete" }] as Array<ActionButtonData>)
  ];

  public override onGridRowClicked(action: ClickedEvent) {
    if (action !== undefined && action.action > "") {
      switch (action.action) {
        case "edit": {
          this.onEdit(action.data as ComunicacionEntity);
          break;
        }
        case "delete": {
          this.onDelete(action.data as ComunicacionEntity);
          break;
        }
        case "clone": {
          let data = new ComunicacionEntity(action.data as ComunicacionEntity);
          data.id = null;
          data.enviado = false;
          data.detalleEnvio = "";
          data.fechaEnvio = new Date();
          data.estado = "D";

          this.onAdd(data);
          break;
        }
        default: {
          alert(action.action);
          break;
        }
      }
    }
  }

  public userToChange: ComunicacionEntity | undefined;

  public getId(data: ComunicacionEntity) {
    return data.id;
  }

  public getLabel(data: ComunicacionEntity) {
    return data.titulo ?? "";
  }

  public override onSearchBefore(): boolean {

    if (this.formSearch !== undefined) {

      const buscar = this.formSearch.value;

      if (buscar.estado !== undefined)
        this.page.filter.push({ property: 'Estado', condition: "=|T", value: buscar.estado } as PagingFilter);

      if (buscar.enviado !== undefined)
        this.page.filter.push({ property: 'Enviado', condition: "=|T", value: buscar.enviado == "true" ? true : (buscar.enviado == "false" ? false : buscar.enviado) } as PagingFilter);

      if (buscar.desde !== undefined && buscar.desde !== null) {
        let date = moment(buscar.desde).toDate();
        this.page.filter.push({
          property: 'FechaEnvio', condition: ">=", value: new Date(date.getFullYear(), date.getMonth(), date.getDate())
        } as PagingFilter);
      }

      if (buscar.hasta !== undefined && buscar.hasta !== null) {
        let date = moment(buscar.hasta).add(1, "days").toDate();
        this.page.filter.push({
          property: 'FechaEnvio', condition: "<=", value: new Date(date.getFullYear(), date.getMonth(), date.getDate())
        } as PagingFilter);
      }
    }

    return true;
  }

  public pageAfiliado: PageGeneric<AfiliadoEntity> = new PageGeneric<AfiliadoEntity>();

  public afiliadosColumnDefs: Array<ColDef | ColGroupDef> = [
    {
      headerName: "Nombre",
      field: "usuarioNombre",
    },
    {
      headerName: "E-Mail",
      field: "email",
      width: 200,
    },
    {
      headerName: "Teléfono",
      field: "celular",
      width: 150,
    },
    {
      headerName: "Edad",
      field: "fechaNacimiento",
      width: 60,
      cellRenderer: (params: ICellRendererParams) => {
        return moment.duration(moment().diff(moment(params.value)), "ms").years();
      },
    },
    {
      headerName: "Ciudad",
      field: "direccion.ciudad",
      width: 60,
    },
    {
      headerName: "Género",
      field: "sexo",
      width: 60,
      cellRenderer: (params: ICellRendererParams) => {
        return this.findElement(params.value, this.generosTodos);
      },
    },
    {
      headerName: "Estado",
      field: "estado",
      width: 60,
      cellRenderer: (params: ICellRendererParams) => {
        return this.findElement(params.value, this.estadosAfiliado);
      },
    },
  ];

  public onSearchAfiliado() {
    this.pageAfiliado.filter = [];
    this.pageAfiliado.order = [];
    this.pageAfiliado.filter.push({ property: 'searchText', condition: 'X', value: this.searchText } as PagingFilter);

    if (this.formSearch2 !== undefined) {

      const buscar = this.formSearch2.value;

      if (buscar.estado !== undefined)
        this.pageAfiliado.filter.push({ property: 'Estado', condition: "=|T", value: buscar.estado } as PagingFilter);

      if (buscar.numeroIdentidad !== undefined && buscar.numeroIdentidad != "")
        this.pageAfiliado.filter.push({ property: 'NumeroIdentidad', condition: "=", value: buscar.numeroIdentidad } as PagingFilter);

      if (buscar.celular !== undefined && buscar.celular != "")
        this.pageAfiliado.filter.push({ property: 'Celular', condition: "=", value: buscar.celular } as PagingFilter);

      if (buscar.email !== undefined && buscar.email != "")
        this.pageAfiliado.filter.push({ property: 'Email', condition: "=", value: buscar.email } as PagingFilter);

      if (buscar.sexo !== undefined && buscar.sexo != "T")
        this.pageAfiliado.filter.push({ property: 'Sexo', condition: "=", value: buscar.sexo } as PagingFilter);

      if (this.edadMin > 0 || this.edadMax < 120) {
        this.pageAfiliado.filter.push({
          property: 'Edad', condition: "X", value: `{"min":${this.edadMin},"max":${this.edadMax}}`
        } as PagingFilter);
      }
    }

    const page: Page = new Page(this.pageAfiliado);
    this.showOverlay();
    this.isSearching = true;

    setTimeout(() => {
      this.appCrud.FindAfiliados(page).subscribe((r: ReturnData) => {
        if (!r.isError) {
          this.pageAfiliado = r.data as PageGeneric<AfiliadoEntity>;
        }
        this.hideOverlay();
        this.isSearching = false;
        this.onSearchAfter(!r.isError, r);
      });
    }, 500);

    return page;
  }

  public onOpenComunicadoAdd() {

    let data: ComunicacionEntity = this.item!;
    let page = new Page(this.pageAfiliado);
    page.data = null;
    data.destinos = JSON.stringify(page);

    this.dialog.open(EdicionComunicacionComponent, {
      data: {
        entity: data, totalAfiliados: this.pageAfiliado.recordCount, onAction: async (entity: any) => {
          let result = await lastValueFrom(this.appCrud.Save(entity));
          if (!result.isError) {
            this.openDialogOk("La operación se realizó con éxito");
            this.onCancel(true);
          }
        }
      },
      minWidth: "800px",
      width: "800px",
      maxWidth: "800px"
    });
  }

  public override onAddAfter() {
    super.onAddAfter();
    this.onSearchAfiliado();
  }

  public override onEditAfter() {
    try {

      let page = JSON.parse(this.item?.destinos ?? "") as Page;

      for (var i = 0; i < page.filter.length; i++) {

        switch (page.filter[i].property) {
          case "searchText": {
            break;
          }
          case "Edad": {
            let d = JSON.parse(page?.filter[i]?.value.toString() ?? "");
            this.edadMin = d.min;
            this.edadMax = d.max;
            break;
          }
          default: {
            this.formSearch2?.controls[page.filter[i].property.toLowerCase()].setValue(page.filter[i].value);
            break;
          }
        }
      }
    }
    catch (error) { }
    super.onEditAfter();
    this.onSearchAfiliado();
  }


  constructor(
    public override router: Router,
    public override activatedRoute: ActivatedRoute,
    public override ref: ChangeDetectorRef,
    public override formBuilder: FormBuilder,
    public override dialog: MatDialog,
    public override modelService: ModelsService,
    public override appCrud: ComunicacionApp,
  ) {
    super(router, activatedRoute, ref, formBuilder, dialog, modelService, Constants.LAYOUT_COMUNICACION, appCrud, ComunicacionEntity, CrudMode.Search);

  }

}
