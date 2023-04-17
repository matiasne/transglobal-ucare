import { LabelType, Options } from '@angular-slider/ngx-slider';
import { HttpClient } from '@angular/common/http';
import { ChangeDetectorRef, Component, QueryList, ViewChildren } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MapInfoWindow, MapMarker } from '@angular/google-maps';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import * as moment from 'moment';
import { lastValueFrom, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { AlertasEntity } from '../../../core/alertas/domain/alertas.entity';
import { MapaConfigEntity } from '../../../core/mapa-config/domain/mapa-config.entity';
import { MapaApp } from '../../../core/mapas/application/mapa.app';
import { AlertComponent } from '../../../shared/alert/alert.component';
import { Constants } from '../../../shared/Constants';
import { CrudControllerComponent, CrudMode, SelectItem } from '../../../shared/controller/controller.service';
import { ModelsService } from '../../../shared/guard/model';
import { PagingFilter, ReturnData } from '../../../shared/rest/rest.service';

@Component({
  selector: 'app-mapa',
  templateUrl: './mapa.component.html',
  styleUrls: ['./mapa.component.scss']
})
export class MapaComponent extends CrudControllerComponent<AlertasEntity> {

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
      desde_time: [0 + (new Date().getTimezoneOffset() * 60000)],
      hasta_time: [(24 * 60 * 60 * 1000) + (new Date().getTimezoneOffset() * 60000) - 1],
    });

  public options: google.maps.MapOptions = {
    center: { lat: 40, lng: -20 },
    zoom: 4,
    
  };

  public markerPositions: MakerIcon[] = [];

  public SinAsignar = "S";
  public Emergencia = "E";
  public Urgencia = "U";
  public FalsaAlarma = "F";

  public estadosAlertas: Array<SelectItem> = [{ id: "S", label: "Sin Asignar" }, { id: "E", label: "Emergencia" }, { id: "U", label: "Urgencia" }, { id: "F", label: "Falsa alarma" }];
  public estadosAlertasTodos: Array<SelectItem> = [{ id: "T", label: "Todas" }, { id: "S", label: "Sin Asignar" }, { id: "E", label: "Emergencia" }, { id: "U", label: "Urgencia" }, { id: "F", label: "Falsa alarma" }];
  public generosTodos: Array<SelectItem> = [{ id: "T", label: "Todos" }, { id: "M", label: "Masculino" }, { id: "F", label: "Femenino" }, { id: "O", label: "Otro" }];

  public apiLoaded: boolean = false;
  public map: google.maps.Map | undefined;

  @ViewChildren(MapInfoWindow) infoWindowsView: QueryList<MapInfoWindow> | undefined;

  public getId(data: AlertasEntity) {
    return data.id;
  }
  public getLabel(data: AlertasEntity) {
    return data.afiliadoNombreCompleto;
  }

  public getSexo(sexo: string) {
    return this.generosTodos.find((item) => {
      return item.id == sexo.substr(0, 1).toUpperCase();
    })?.label ?? "";
  }

  public override onSearchBefore(): boolean {
    if (this.formSearch !== undefined && this.formSearch !== null) {
      var buscar = this.formSearch.value;

      if (buscar.estado !== undefined && buscar.estado !== null) {
        this.page.filter.push({
          property: 'Estado', condition: "=|T", value: buscar.estado
        } as PagingFilter);
      }

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

      let horasInMuilisecons = 60 * 60 * 1000;
      let minutosInMuilisecons = 60 * 1000;
      let segundosInMuilisecons = 1000;
      let offset = new Date().getTimezoneOffset();

      if (buscar.desde_time !== undefined && buscar.desde_time !== null) {
        let date = moment(buscar.desde_time).toDate();
        this.page.filter.push({
          property: 'desde_time', condition: "x", value: (date.getHours() * horasInMuilisecons) + ((date.getMinutes() + offset) * minutosInMuilisecons) + (date.getSeconds() * segundosInMuilisecons)
        } as PagingFilter);
      }

      if (buscar.hasta_time !== undefined && buscar.hasta_time !== null) {
        let date = moment(buscar.hasta_time).toDate();
        this.page.filter.push({
          property: 'hasta_time', condition: "x", value: (date.getHours() * horasInMuilisecons) + ((date.getMinutes() + offset) * minutosInMuilisecons) + (date.getSeconds() * segundosInMuilisecons)
        } as PagingFilter);
      }

      this.page.filter.push({
        property: 'offset', condition: "x", value: (offset * minutosInMuilisecons)
      } as PagingFilter);
    }

    return true;
  }

  public override onSearchAfter(isOk: boolean, r: ReturnData): void {
    super.onSearchAfter(isOk, r);
    if (isOk) {
      this.markerPositions = [];
      for (const element of this.page.data) {
        let item = element;
        let iconUrl: string = "";
        switch (item.estado) {
          case "E":
            {
              iconUrl = "assets/images/mapa/icono_eme.svg";
              break;
            }
          case "U":
            {
              iconUrl = "assets/images/mapa/icono_urg.svg";
              break;
            }
          case "F":
            {
              iconUrl = "assets/images/mapa/icono_fal.svg";
              break;
            }
          default: {
            iconUrl = "assets/images/mapa/icono_sin.svg";
            break;
          }
        }
        this.markerPositions.push(
          {
            data: item,
            position: { lat: item.position.lat, lng: item.position.lon } as google.maps.LatLngLiteral,
            option: {
              draggable: false,
              icon: {
                url: iconUrl,
                size: new google.maps.Size(48, 60),
                origin: new google.maps.Point(0, 0),
                anchor: new google.maps.Point(24, 60),
                //scaledSize: new google.maps.Size(25, 25)
              },
              title: item.afiliadoNombreCompleto,

            } as google.maps.MarkerOptions
          } as MakerIcon);
      }
    }
  }

  private center: any;
  private zoom: any;

  public onMapInitialized(map: any) {
    this.map = map as google.maps.Map;
  }
  public onChangeCenter() {
    this.center = { lat: this.map?.getCenter()?.lat(), lon: this.map?.getCenter()?.lng() };
  }
  public onChangeZoom() {
    this.zoom = this.map?.getZoom();
  }
  public onSaveConfig() {

    const diag = this.dialog.open(AlertComponent, { data: { titulo: "Guardar", label: "¿Está seguro de querer guardar esta modificación?", type: "question" }, panelClass: ['mo-alert'], autoFocus: false });
    const subscrib = diag.afterClosed().subscribe(async (result) => {
      subscrib.unsubscribe();
      if (result) {
        let data = {
          center: { lat: this.center.lat, lon: this.center.lon },
          zoom: this.zoom
        } as MapaConfigEntity;

        let resultSave = await lastValueFrom(this.appCrud.SaveConfig(data));
        if (!resultSave.isError) {
          this.openDialogOk("Se guardó la configuración con éxito");
        }
        else {
          this.openDialogError("Error inesperado");
        }
      }
    });
  }

  //ABRIMOS LA INFO DE UN MARKER
  public openInfoWindow(marker: MapMarker, item: MakerIcon, windowIndex: number) {
    let curIdx = 0;
    this.infoWindowsView!.forEach((window: MapInfoWindow) => {
      if (windowIndex === curIdx) {
        //SI esta en estado de emergencia obtenemos mas informacion del usuario
        if (item.data.estado === this.Emergencia) {
          this.modelService.showHourglass = true;
          //this.appCrud.getMoreInfo(item.data.id).toPromise().then(resultInfo => {
          //  if (!resultInfo?.isError) {
          //    item.data.moreInfo = resultInfo?.data;
          //  }
          //  else
          //    item.data.moreInfo = null;
          //}).finally(() => {
          window.open(marker);
          this.modelService.showHourglass = false;
          //})
        }
        else {
          window.open(marker);
        }
        curIdx++;
      } else {
        curIdx++;
      }
    });
  }

  //COPIAMOS AL PORTAPAPELES UN TEXTO
  public onCopyContent(val: string, $event: Event) {
    $event.stopPropagation();
    $event.stopImmediatePropagation();
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

  public ago(fecha: any) {
    return moment(fecha).fromNow()
  }

  public fecha(date: any) {
    //return moment(date).add(new Date().getTimezoneOffset(), "minutes").format("DD/MM/YYYY HH:mm");
    return moment(date).format("DD/MM/YYYY HH:mm");
  }

  constructor(
    public override router: Router,
    public override activatedRoute: ActivatedRoute,
    public override ref: ChangeDetectorRef,
    public override formBuilder: FormBuilder,
    public override dialog: MatDialog,
    public override modelService: ModelsService,
    public override appCrud: MapaApp,
    httpClient: HttpClient) {

    super(router, activatedRoute, ref, formBuilder, dialog, modelService, Constants.LAYOUT_MAPA, appCrud, AlertasEntity, CrudMode.Search);

    appCrud.getMapConfig().toPromise().then(async (data) => {

      let zoomDefault = 15;
      let latDefault = -31.4;
      let lonDefault = -64.2;

      let lat = data?.data?.center?.lat ?? latDefault;
      lat = (lat < -90 || lat > 90) ? latDefault : lat;

      let lon = data?.data?.center?.lon ?? lonDefault;
      lon = (lon < -180 || lon > 180) ? lonDefault : lon;

      let zoom = data?.data?.zoom ?? zoomDefault;
      zoom = (zoom < 2 || zoom > 20) ? zoomDefault : zoom;

      this.center = { lat: lat, lng: lon };
      this.zoom = zoom;
      this.options = {
        center: { lat: lat, lng: lon },
        zoom: zoom,
      } as google.maps.MapOptions;

      this.apiLoaded = await lastValueFrom(httpClient.jsonp('https://maps.googleapis.com/maps/api/js?key=' + (data?.data?.apiKey ?? ""), 'callback')
        .pipe(
          map(() => true),
          catchError((err) =>
            of(false)),
        ));
    })

  }

}

class MakerIcon {

  constructor(public data: any,
    public position: google.maps.LatLngLiteral,
    public option: google.maps.MarkerOptions) {

  }
}
