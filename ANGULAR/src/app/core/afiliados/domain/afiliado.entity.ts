import * as moment from "moment";
import { Entity } from "../../../shared/domain/Entity";

export class AfiliadoEntity extends Entity {

  constructor(data: any = undefined) {
    super(data);

    if (data !== undefined && data !== null) {
      this.id = data.id;
      this.usuarioNombre = data.usuarioNombre;
      this.email = data.email;
      this.numeroIdentidad = data.numeroIdentidad;
      this.fechaNacimiento = moment(data.fechaNacimiento).subtract(moment(data.fechaNacimiento).utcOffset(), "minutes").toDate();
      this.sexo = data.sexo;
      this.direccion = data.direccion;
      this.position = data.position;
      this.celular = data.celular;
      this.codigoPais = data.codigoPais;
      this.nosocomio = data.nosocomio;
      this.patologias = data.patologias;
      this.medicacion = data.medicacion;
      this.alergias = data.alergias;
      this.contactos = data.contactos;
      this.afiliacion = data.afiliacion;
      this.creado = data.creado;
      this.modificacion = data.modificacion;
      this.estado = data.estado;
    }
  }

  public id: string | null = null;
  public usuarioNombre: string = "";
  public email: string = "";
  public numeroIdentidad: string = "";
  public fechaNacimiento: Date | null = null;
  public sexo: string = "";
  public direccion: DireccionEntity = new DireccionEntity();
  public position: GeoPositionEntity = new GeoPositionEntity();
  public celular: string = "";
  public codigoPais: string = "";
  public nosocomio: string = "";
  public patologias: Array<string> = [];
  public medicacion: Array<string> = [];
  public alergias: Array<string> = [];
  public contactos: Array<ContactoEntity> = [];
  public afiliacion: AfiliacionEntity = new AfiliacionEntity();
  public creado: any = "";
  public modificacion: any = "";

  public estado: string = "A";

}

export class DireccionEntity extends Entity {
  public calle: string = "";
  public nro: string = "";
  public piso: string = "";
  public barrio: string = "";
  public ciudad: string = "";
  public departamento: string = "";
  public codigoPostal: string = "";
}

export class GeoPositionEntity extends Entity {
  public lat: number = 0;
  public lon: number = 0;
}

export class ContactoEntity extends Entity {
  public nombre: string = "";
  public telefono: string = "";
}

export class AfiliacionEntity extends Entity {
  public empresa: string = "";
  public servicio: string = "";
}
