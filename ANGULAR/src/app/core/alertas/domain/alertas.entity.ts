import { Entity } from "../../../shared/domain/Entity";

export class AlertasEntity extends Entity {

  constructor(data: any = undefined) {
    super(data);

    if (data !== undefined && data !== null) {
      this.id = data.id;
      this.afiliadoId = data.afiliadoId;
      this.afiliadoNombreCompleto = data.afiliadoNombreCompleto;
      this.afiliadoTelefonoContacto = data.afiliadoTelefonoContacto;
      this.afiliadoUbicacion = data.afiliadoUbicacion;
      this.afiliadoSexo = data.afiliadoSexo;
      this.afiliadoEdad = data.afiliadoEdad;
      this.afiliadoNosocomio = data.afiliadoNosocomio;
      this.bitacora = data.bitacora;
      this.position = new GeoPosition(data.position);
      this.estado = data.estado;
      this.creado = data.creado;
      this.modificacion = data.modificacion;
      this.monitorId = data.monitorId;
      this.confirmaAsignacion = data.confirmaAsignacion;
      this.cerrado = data.cerrado;
      this.alarmaSonora = data.alarmaSonora;

      this.timeOut = data.timeOut ?? 0;
      this.ticks = data.ticks ?? 0;

      this.countDownText = data.countDownText ?? "";

    }
  }

  public id: string | null = null;
  public afiliadoId: string = "";
  public afiliadoNombreCompleto: string = "";
  public afiliadoTelefonoContacto: string = "";
  public afiliadoUbicacion: string = "";
  public afiliadoSexo: string = "";
  public afiliadoEdad: number = 0;
  public afiliadoNosocomio: string = "";
  public bitacora: string = "";
  public position: GeoPosition = new GeoPosition();
  public estado: string = "A";
  public creado: any;
  public modificacion: any;
  public monitorId: string | null | undefined;
  public confirmaAsignacion: boolean = false;
  public cerrado: boolean = false;
  public alarmaSonora: boolean = false;

  public timeOut = 0;
  public ticks = 0;

  public countDownText: string = "";
}

export class GeoPosition extends Entity {

  constructor(data: any = undefined) {
    super(data);

    if (data !== undefined && data !== null) {
      this.lat = data.lat;
      this.lon = data.lon;
    }

  }
  public lat: number | null = null;
  public lon: number | null = null;
}
