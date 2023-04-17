import { Entity } from "../../../shared/domain/Entity";

export class ComunicacionEntity extends Entity {

  constructor(data: any = undefined) {
    super(data);

    if (data !== undefined && data !== null) {
      this.id = data.id;
      this.titulo = data.titulo;
      this.mensaje = data.mensaje;
      this.fechaEnvio = data.fechaEnvio;
      this.enviado = data.enviado;
      this.destinos = data.destinos;
      this.estado = data.estado;
      this.detalleEnvio = data.detalleEnvio;
      this.creado = data.creado;
      this.modificacion = data.modificacion;
    }
  }

  public id: string | null = null;
  public titulo: string = "";
  public mensaje: string = "";
  public fechaEnvio: Date | null = new Date();
  public enviado: boolean = false;
  public destinos: string = "";
  public estado: string = "A";
  public detalleEnvio: string = "";
  public creado: any;
  public modificacion: any;

}
