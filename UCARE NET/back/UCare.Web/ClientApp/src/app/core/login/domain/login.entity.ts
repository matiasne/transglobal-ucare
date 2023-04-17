import { Entity } from "../../../shared/domain/Entity";

export class LoginEntity extends Entity {

  constructor(data: any) {
    super();

    if (data !== undefined && data !== null) {
      this.id = data.id;
      this.email = data.email;
      this.name = data.name;
      this.rol = data.rol;
    }

  }
  public id: string = "";
  public name: string = "";
  public email: string = "";
  public rol: string = "";

}
