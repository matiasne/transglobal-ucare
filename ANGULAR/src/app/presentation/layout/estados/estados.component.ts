import { LabelType, Options } from '@angular-slider/ngx-slider';
import { ChangeDetectorRef, Component } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { ColDef, ColGroupDef, ICellRendererParams } from 'ag-grid-community';
import * as moment from 'moment';
import { AlertasEntity } from '../../../core/alertas/domain/alertas.entity';
import { EstadosApp } from '../../../core/estados/application/estados.app';
import { Constants } from '../../../shared/Constants';
import { CrudControllerComponent, CrudMode, SelectItem } from '../../../shared/controller/controller.service';
import { ClickedEvent } from '../../../shared/grid/grid.component';
import { ModelsService } from '../../../shared/guard/model';
import { PagingFilter } from '../../../shared/rest/rest.service';
import { ShowBitacoraComponent } from './dialogs/show-bitacora.component';

@Component({
  selector: 'app-estados',
  templateUrl: './estados.component.html',
  styleUrls: ['./estados.component.scss']
})
export class EstadosComponent extends CrudControllerComponent<AlertasEntity> {
  public IsEmergencias: boolean = true;
  public IsUrgencias: boolean = false;
  public IsFalsaAlarma: boolean = false;

  public SinAsignar = "S";
  public Emergencia = "E";
  public Urgencia = "U";
  public FalsaAlarma = "F";

  public estadosAlertas: Array<SelectItem> = [{ id: "S", label: "Sin Asignar" }, { id: "E", label: "Emergencia" }, { id: "U", label: "Urgencia" }, { id: "F", label: "Falsa alarma" }];
  public estadosAlertasTodos: Array<SelectItem> = [{ id: "T", label: "Todas" }, { id: "S", label: "Sin Asignar" }, { id: "E", label: "Emergencia" }, { id: "U", label: "Urgencia" }, { id: "F", label: "Falsa alarma" }];
  public generosTodos: Array<SelectItem> = [{ id: "T", label: "Todos" }, { id: "M", label: "Masculino" }, { id: "F", label: "Femenino" }, { id: "O", label: "Otro" }];


  public getId(data: AlertasEntity) {
    return data.id;
  }
  public getLabel(data: AlertasEntity) {
    return data.afiliadoNombreCompleto;
  }

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
          return ''+value;
      }
    }
  };

  public override formSearch: FormGroup | undefined = this.formBuilder.group(
    {
      estado: ['T'],
      edad: [null],
      genero: ['T'],
      desde: [moment().toDate()],
      hasta: [moment().toDate()],
    });

  public override columnDefs: Array<ColDef | ColGroupDef> | undefined = [
    {
      headerName: "",
      field: "estado",
      width: 42,
      maxWidth: 42,
      minWidth: 42,
      cellRenderer: (params: ICellRendererParams) => {
        switch (params.value) {
          case "E":
            return '<div style="width: 24px;height: 24px; border: none;border-radius: 12px; background-color: #EF233C;"></div>';
          case "U":
            return '<div style="width: 24px;height: 24px; border: none;border-radius: 12px; background-color: #EF9D23;"></div>';
          case "F":
            return '<div style="width: 24px;height: 24px; border: none;border-radius: 12px; background-color: #6B0BF3;"></div>';
          default:
            return '<div style="width: 24px;height: 24px; border: none;border-radius: 12px; background-color: #999;"></div>';
        }
      }
    },
    {
      headerName: "Nombre",
      field: "afiliadoNombreCompleto",
    },
    {
      headerName: "Ubicacion",
      field: "afiliadoUbicacion",
    },
    {
      headerName: "Fecha",
      field: "creado.modificado",
      sortable: true,
      cellRenderer: (params: ICellRendererParams) => {
        return moment(params.value).format("DD/MM/YYYY")
      }
    },
    {
      headerName: "Hora",
      field: "creado.modificado",
      cellRenderer: (params: ICellRendererParams) => {
        return moment(params.value).format("HH:mm:SS")
      }
    },
    {
      headerName: "Cerrado",
      field: "cerrado",
      cellRenderer: (params: ICellRendererParams) => {
        return params.value ? "SI" : "NO";
      }
    },
    //{
    //  headerName: "Edad",
    //  field: "creado",
    //},
    //{
    //  headerName: "Sexo",
    //  field: "creado",
    //},
    {
      headerName: "Bitácora",
      width: 80,
      maxWidth: 80,
      minWidth: 80,
      suppressMenu: true,
      sortable: false,
      suppressMovable: true,
      suppressAutoSize: true,
      resizable: false,
      pinned: "right",
      cellRenderer: "actionButtons",
      cellRendererParams: { buttons: [{ icon: "article", title: "Ver", action: "bitacora" }] }
    }
  ];

  public override onGridRowClicked(action: ClickedEvent) {
    if (action !== undefined && action.action > "") {
      switch (action.action) {
        case "bitacora": {
          this.dialog.open(ShowBitacoraComponent, {
            data: (action.data as AlertasEntity).bitacora, maxWidth: "800px"
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


    if (this.IsEmergencias || this.IsUrgencias || this.IsFalsaAlarma) {

      let arr = [];

      if (this.IsEmergencias) arr.push(this.Emergencia);
      if (this.IsUrgencias) arr.push(this.Urgencia);
      if (this.IsFalsaAlarma) arr.push(this.FalsaAlarma);

      this.page.filter.push({
        property: 'Estado', condition: "x", value: arr
      } as PagingFilter);
    }

    if (this.formSearch !== undefined && this.formSearch !== null) {
      let buscar = this.formSearch.value;

      if (this.edadMin > 0) {
        this.page.filter.push({
          property: 'AfiliadoEdadMin', condition: "x", value: this.edadMin
        } as PagingFilter);
      }

      if (this.edadMax < 120) {
        this.page.filter.push({
          property: 'AfiliadoEdadMax', condition: "x", value: this.edadMax
        } as PagingFilter);
      }

      if (buscar.genero !== undefined && buscar.genero !== null) {
        this.page.filter.push({
          property: 'AfiliadoSexo', condition: "=|T", value: buscar.genero
        } as PagingFilter);
      }

      if (buscar.desde !== undefined && buscar.desde !== null) {
        let date = moment(buscar.desde).toDate();
        this.page.filter.push({
          property: 'Creado.Modificado', condition: ">=", value: new Date(date.getFullYear(), date.getMonth(), date.getDate())
        } as PagingFilter);
      }

      if (buscar.hasta !== undefined && buscar.hasta !== null) {
        let date = moment(buscar.hasta).add(1, "days").toDate();
        this.page.filter.push({
          property: 'Creado.Modificado', condition: "<=", value: new Date(date.getFullYear(), date.getMonth(), date.getDate())
        } as PagingFilter);
      }
    }

    return true;
  }


  public onSelectEmergencias() {
    this.IsEmergencias = !this.IsEmergencias;
    this.IsUrgencias = false;
    this.IsFalsaAlarma = false;
    this.onSearch(true);
  }

  public onSelectUrgencias() {
    this.IsUrgencias = !this.IsUrgencias;
    this.IsEmergencias = false;
    this.IsFalsaAlarma = false;
    this.onSearch(true);
  }

  public onSelectFalsaAlarma() {
    this.IsFalsaAlarma = !this.IsFalsaAlarma;
    this.IsUrgencias = false;
    this.IsEmergencias = false;
    this.onSearch(true);
  }

  constructor(
    public override router: Router,
    public override activatedRoute: ActivatedRoute,
    public override ref: ChangeDetectorRef,
    public override formBuilder: FormBuilder,
    public override dialog: MatDialog,
    public override modelService: ModelsService,
    public override appCrud: EstadosApp,
  ) {
    super(router, activatedRoute, ref, formBuilder, dialog, modelService, Constants.LAYOUT_ESTADOS, appCrud, AlertasEntity, CrudMode.Search);
  }

}
