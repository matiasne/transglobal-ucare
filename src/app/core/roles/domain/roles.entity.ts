import { Entity } from "../../../shared/domain/Entity";

export class RolesEntity extends Entity {

  constructor(data: any = undefined) {
    super(data);

    if (data !== undefined && data !== null) {
      this.id = data.id;
      this.usuarioNombre = data.usuarioNombre;
      this.email = data.email;
      this.password = data.password;
      this.salt = data.salt;
      this.rol = data.rol;
      this.codigoPostal = data.codigoPostal;
      this.usuarioId = data.usuarioId;
      this.estado = data.estado;
    }

  }
  public id: string | null = null;
  public usuarioNombre: string = "";
  public email: string = "";
  public password: string = "";
  public salt: string = "";
  public rol: string = "";
  public codigoPostal: Array<string> = [];
  public usuarioId: string = "";
  public estado: string = "A";

}
