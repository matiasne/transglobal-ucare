import { HttpClient } from '@angular/common/http';
import { ChangeDetectorRef, Component, Injector, QueryList, ViewChildren } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MapInfoWindow, MapMarker } from '@angular/google-maps';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpTransportType, HubConnection, HubConnectionBuilder, IHttpConnectionOptions, DefaultHttpClient, HttpRequest, HttpResponse } from '@microsoft/signalr';
import * as moment from 'moment';
import { lastValueFrom, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { environment } from '../../../../environments/environment';
import { AlertasEntity } from '../../../core/alertas/domain/alertas.entity';
import { MapaMonitorApp } from '../../../core/mapas-monitor/application/mapas-monitor.app';
import { Constants } from '../../../shared/Constants';
import { CrudControllerComponent, CrudMode, SelectItem } from '../../../shared/controller/controller.service';
import { ModelsService } from '../../../shared/guard/model';
import { Page } from '../../../shared/rest/rest.service';
import { AlarmaActivarAlertaComponent } from './dialogs/alarma-activar-alerta.component';
import { ChangeEstadoAlertaComponent } from './dialogs/change-estado-alerta.component';
import { CountDownPauseAlertaComponent } from './dialogs/count-down-pause.component';
import { MonitorPausaComponent } from './dialogs/monitor-pausa.component';
import { WriteAlertaBitacoraComponent } from './dialogs/write-bitacora-alerta.component';

@Component({
  selector: 'app-mapa-monitor',
  templateUrl: './mapa-monitor.component.html',
  styleUrls: ['./mapa-monitor.component.scss']
})
export class MapaMonitorComponent extends CrudControllerComponent<AlertasEntity> {

  public options: google.maps.MapOptions = {
    center: { lat: 40, lng: -20 },
    zoom: 4,
  };
  public monitoState: Monitor | undefined;

  public markerPositions: MakerIcon[] = [];
  public estadosAlertas: Array<SelectItem> = [{ id: "E", label: "Emergencia" }, { id: "U", label: "Urgencia" }, { id: "F", label: "Falsa alarma" }];
  public SinAsignar = "S";
  public Emergencia = "E";
  public Urgencia = "U";
  public FalsaAlarma = "F";

  public estado: string | undefined = "";

  public apiLoaded: boolean = false;
  public map: google.maps.Map | undefined;

  private connection: HubConnection | undefined;

  public connectionState = false;
  public isJoin = false;
  public asignados: Array<AlertasEntity> = [];

  @ViewChildren(MapInfoWindow) infoWindowsView: QueryList<MapInfoWindow> | undefined;
  public alertInterval: any = {};
  public audio: HTMLAudioElement;
  public baseUrl: string;

  constructor(
    public override router: Router,
    public override activatedRoute: ActivatedRoute,
    public override ref: ChangeDetectorRef,
    public override formBuilder: FormBuilder,
    public override dialog: MatDialog,
    public override modelService: ModelsService,
    public override appCrud: MapaMonitorApp,
    protected injector: Injector,
    public httpClient: HttpClient) {

    super(router, activatedRoute, ref, formBuilder, dialog, modelService, Constants.LAYOUT_MONITOREO, appCrud, AlertasEntity, CrudMode.None);

    this.baseUrl = injector.get('BASE_URL').trim();

    this.audio = new Audio();
    this.audio.src = this.baseUrl + "assets/sounds/recive.wav";
    this.audio.load();

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
    });

    this.createHub();

  }

  protected getToken(): string | null {
    let token = null;
    try {
      token = localStorage.getItem("sessionToken");
    } catch (e) {
    }
    return token;
  }

  public createHub(skipNegotiation = true) {

    if (this.connection !== undefined && this.connection !== null) {
      this.connection.off("OnSelectAlerta");
      this.connection.off("OnAddNewAlerta");
      this.connection.off("OnChangeAlerta");
      this.connection.off("OnChangeMonitorStatus");
      this.connection.off("OnDisconectedMonitor");

    }

    this.connection = new HubConnectionBuilder()
      .withUrl((environment.production ? this.baseUrl + `monitor_hub` : `monitor_hub`),
        {
          accessTokenFactory: () => {
            return this.getToken();
          },
          skipNegotiation: skipNegotiation,
          timeout: 30,
          transport: HttpTransportType.WebSockets
        } as IHttpConnectionOptions)
      .build();

    this.connection.on("OnSelectAlerta", message => this.OnSelectAlerta(message));
    this.connection.on("OnAddNewAlerta", message => this.OnAddNewAlerta(message));
    this.connection.on("OnChangeAlerta", message => this.OnChangeAlerta(message));
    this.connection.on("OnChangeMonitorStatus", message => this.OnChangeMonitorStatus(message));
    this.connection.on("OnDisconectedMonitor", message => window.location.href = this.baseUrl + "login");

  }

  public getId(data: AlertasEntity) {
    return data.id;
  }
  public getLabel(data: AlertasEntity) {
    return data.afiliadoNombreCompleto;
  }

  public override onSearch(excecuteSearch = true): Page | null {
    return null;
  }

  //ABRIMOS LA INFO DE UN MARKER
  public openInfoWindow(marker: MapMarker, item: MakerIcon, windowIndex: number) {
    let curIdx = 0;
    this.infoWindowsView!.forEach((window: MapInfoWindow) => {
      if (windowIndex === curIdx) {
        //SI esta en estado de emergencia obtenemos mas informacion del usuario
        if (item.data.estado === this.Emergencia) {
          this.modelService.showHourglass = true;
          this.appCrud.getMoreInfo(item.data.id).toPromise().then(resultInfo => {
            if (!resultInfo?.isError) {
              item.data.moreInfo = resultInfo?.data;
            }
            else
              item.data.moreInfo = null;
          }).finally(() => {
            window.open(marker);
            this.modelService.showHourglass = false;
          })
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

  //Agrega los markers nuevos
  public pushMarker(item: AlertasEntity) {
    let iconUrl: string = "";
    switch (item.estado) {
      case this.Emergencia:
        {
          iconUrl = "assets/images/mapa/icono_eme.svg";
          break;
        }
      case this.Urgencia:
        {
          iconUrl = "assets/images/mapa/icono_urg.svg";
          break;
        }
      case this.FalsaAlarma:
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

  public onMapInitialized(map: any) {
    this.map = map as google.maps.Map;
  }
  countReintentos = 0;
  override async ngOnInit() {
    super.ngOnInit();

    this.modelService.showHourglass = true;
    this.connectionState = false;
    this.connection!.start()
      .then(_ => {
        console.log('Connection Started');
        this.IsJoin().then(val => {
          this.isJoin = val;
          this.connectionState = true;
        }).catch(error => {
          console.error(error);
          setTimeout(this.ngOnInit.bind(this), 10000);
        }).finally(() => {
          this.countReintentos = 0;
          this.modelService.showHourglass = false;
        });

      }).catch(error => {
        console.error(error);

        this.countReintentos++;

        if ((this.countReintentos % 2) == 0) {
          this.createHub(false);
        }
        else {
          this.createHub(true);
        }

        setTimeout(this.ngOnInit.bind(this), 10000);

      });

    setTimeout(() => {
      this.appCrud.getAsignados().toPromise().then((result) => {
        this.asignados = result?.data ?? [];
        this.asignados.forEach(item => {
          this.countDown(item);
          this.replaceMarker(item);
          this.playAudio();
        })
      });
    }, 2000);

  }

  public onJoin() {
    this.connection!.invoke<boolean>("Join").then(result => {
      this.isJoin = result;
    }).catch(error => {
      console.error(error);
      setTimeout(this.ngOnInit.bind(this), 10000);
    }).finally(() => {
      this.modelService.showHourglass = false;
    });
  }

  public IsJoin(): Promise<boolean> {
    return this.connection!.invoke<boolean>("IsJoin");
  }

  public onConfirmaAsignacion(alertaId: string) {
    this.connection!.invoke<boolean>("ConfirmarAsignacion", alertaId).then(result => {
      if (!result) {
        this.openDialogError("No se pudo realizar la accion");
      }
    }).finally(() => {
      this.modelService.showHourglass = false;
    });
  }

  public onCambiarEstado(alertaId: string, estado: string) {
    this.connection!.invoke<boolean>("CambiarEstado", alertaId, estado).then(result => {
      if (!result) {
        this.openDialogError("No se pudo realizar la accion");
      }
    }).finally(() => {
      this.modelService.showHourglass = false;
    });
  }

  public onPausar() {
    this.connection!.invoke<boolean>("Descanso").then(result => {
      if (!result) {
        this.openDialogError("No se pudo realizar la accion");
      }
    }).finally(() => {
      this.modelService.showHourglass = false;
    });
  }

  public onIniciar() {
    this.connection!.invoke<boolean>("Iniciar").then(result => {
      if (!result) {
        this.openDialogError("No se pudo realizar la accion");
      }
    }).finally(() => {
      this.modelService.showHourglass = false;
    });
  }

  public onAlertaActivarAlarma(alertaId: string) {
    this.connection!.invoke<boolean>("ActivarDesactivarAlarma", alertaId).then(result => {
      if (!result) {
        this.openDialogError("No se pudo realizar la accion");
      }
    }).finally(() => {
      this.modelService.showHourglass = false;
    });
  }

  public onFinalizarAlerta(alertaId: string, bitacora: string) {
    this.connection!.invoke<boolean>("FinalizarAsistencia", alertaId, bitacora).then(result => {
      if (!result) {
        this.openDialogError("No se pudo realizar la accion");
      }
    }).catch(this.onErrorHub).finally(() => {
      this.modelService.showHourglass = false;
    });
  }

  public onLeave() {
    this.connection!.send("Leave");
  }

  public onErrorHub(err: any) {
    this.openDialogError(err.message!);
  }

  //remplaza los markes mientras se actualizan
  public replaceMarker(message: AlertasEntity) {
    //buscamos si existe el marker
    let marker = this.markerPositions.find(item => {
      return item.data.id === message.id;
    })
    //si existe lo borramos
    if (marker !== undefined && marker !== null && marker.data.id === message.id) {
      this.markerPositions.splice(this.markerPositions.indexOf(marker), 1);
    }
    //agregamos el marker si no esta cerrado
    if (this.modelService.user?.id == message.monitorId && !(message.cerrado ?? false))
      this.pushMarker(message);

    //buscamos si tiene esta asignacion
    let find = this.asignados.find(item => {
      return item.id === message.id;
    });
    //si la tiene la borramos
    if (find !== undefined && find !== null && find?.id === message.id) {
      this.asignados.splice(this.asignados.indexOf(find), 1);
    }
    //chequeamos que sea para el y se la asignamos
    if (this.modelService.user?.id == message.monitorId && !(message.cerrado ?? false))
      this.asignados.unshift(message);
  }

  playAudio() {
    this.audio.play();
  }

  public OnSelectAlerta(message: AlertasEntity) {
    this.countDown(message);
    this.replaceMarker(message);
    this.playAudio();
  }

  public OnAddNewAlerta(message: AlertasEntity) {
    this.replaceMarker(message);
  }

  public OnChangeAlerta(message: AlertasEntity) {
    this.countDown(message);
    this.replaceMarker(message);
  }

  public OnChangeMonitorStatus(message: Monitor) {
    this.monitoState = message;
    switch (message.state) {
      case MonitorState.None: {
        break;
      }
      case MonitorState.Waiting: {
        let date = this.convertTicksToDate(message.timeOut as number);
        if (date.getTime() - (5000) > new Date().getTime()) {
          let modal = this.dialog.open(CountDownPauseAlertaComponent, {
            data: date
            , maxWidth: "800px"
          });
          modal.afterClosed().subscribe((result) => {
            if (result) {
              this.modelService.showHourglass = true;
              this.onPausar();
            }
            else {
              this.onIniciar();
            }
          });
        }
        break;
      }
      case MonitorState.Asigned: {
        break;
      }
      case MonitorState.Disconected:
      case MonitorState.Pause: {
        let date = this.convertTicksToDate(message.ticks!, true)!;
        let modal = this.dialog.open(MonitorPausaComponent, {
          data: {
            date: date,
            data: message
          }
          , maxWidth: "800px"
        });
        modal.afterClosed().subscribe((result) => {
          if (result != null) {
            this.modelService.showHourglass = true;
            this.onIniciar();
          }
          else {
            this.OnChangeMonitorStatus(message);
          }
        });
        break;
      }
    }
  }
  convertTicksToDate(ticks: number, useOffset = false) {
    try {
      return new Date(ticks / 1e+4 + new Date('0001-01-01T00:00:00Z').getTime() + (useOffset ? (new Date().getTimezoneOffset() * 60000) : 0)); //
    }
    catch {
      return new Date(0);
    }
  }

  public onClickAlertPanel(item: AlertasEntity) {
    this.map?.setCenter({ lat: item.position.lat, lng: item.position.lon } as google.maps.LatLngLiteral);
    this.map?.setZoom(this.options.zoom!);
  }

  public ago(fecha: any) {
    return moment(fecha).fromNow()
  }

  public countDown(alerta: AlertasEntity) {

    if (this.alertInterval[alerta.id!] !== undefined && this.alertInterval[alerta.id!] !== null) {
      clearInterval(this.alertInterval[alerta.id!]);
    }

    if (alerta.confirmaAsignacion !== true) {
      let fechaMax = moment(this.convertTicksToDate(alerta.timeOut));
      let duration = moment.duration(fechaMax.toDate().getTime() - (new Date()).getTime(), 'milliseconds');

      this.alertInterval[alerta.id!] = setInterval(() => {
        duration = duration.subtract(1000, 'milliseconds');
        alerta.countDownText = duration.hours() + ":" + duration.minutes() + ":" + duration.seconds();
        try {
          if (duration.asSeconds() < 2) {
            this.asignados.splice(this.asignados.indexOf(alerta), 1);
            if (this.alertInterval[alerta.id!] !== undefined && this.alertInterval[alerta.id!] !== null) {
              clearInterval(this.alertInterval[alerta.id!]);
            }
          }
        } catch { }
      }, 1000);
    }
  }


  public onCambiarSituacion(item: AlertasEntity) {
    let modal = this.dialog.open(ChangeEstadoAlertaComponent, {
      data: { estado: this.estado, alerta: item }
      , maxWidth: "800px"
    });
    modal.afterClosed().subscribe((result) => {
      if (result != null) {
        this.modelService.showHourglass = true;
        this.onCambiarEstado(result.alerta.id, result.estado);
      }
    });

    this.estado = "";
  }

  public onConfirmar(item: AlertasEntity) {
    this.onClickAlertPanel(item);
    this.onConfirmaAsignacion(item.id!);
  }

  public onFinalizar(item: AlertasEntity) {
    let modal = this.dialog.open(WriteAlertaBitacoraComponent, {
      data: item
      , maxWidth: "800px"
    });
    modal.afterClosed().subscribe((result) => {
      if (result != null) {
        this.modelService.showHourglass = true;
        this.onFinalizarAlerta(result.id, result.bitacora);
      }
    });
  }

  public onActivarAlarma(item: AlertasEntity) {
    let modal = this.dialog.open(AlarmaActivarAlertaComponent, {
      data: item
      , maxWidth: "800px"
    });
    modal.afterClosed().subscribe((result) => {
      if (result != null) {
        this.modelService.showHourglass = true;
        this.onAlertaActivarAlarma(result.id);
      }
      else {
        item.alarmaSonora = !item.alarmaSonora;
      }
    });
  }
}

export class MonitorState {
  public static readonly None = 0;
  public static readonly Waiting = 1;
  public static readonly Asigned = 2;
  public static readonly Pause = 3;
  public static readonly Disconected = 4;
}

export class Monitor {
  public id: string | undefined;
  public connectionId: string | undefined;
  public state: number | undefined;
  public ticks: number | undefined;
  public timeOut: number | undefined;
}

class MakerIcon {

  constructor(public data: any,
    public position: google.maps.LatLngLiteral,
    public option: google.maps.MarkerOptions) {

  }
}
