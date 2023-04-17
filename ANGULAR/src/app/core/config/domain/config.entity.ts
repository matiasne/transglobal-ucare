import { Entity } from "../../../shared/domain/Entity";

export class ConfigEntity extends Entity {

  constructor(data: any = undefined) {
    super(data);

    if (data !== undefined && data !== null) {
      this.id = data.id;
      this.usuarioActivosMaximos = data.usuarioActivosMaximos;
      this.tiempoEnvioSMSSeconds = data.tiempoEnvioSMSSeconds;
      this.monitorPausaTimeOut = data.monitorPausaTimeOut;
      this.confirmarTimeOut = data.confirmarTimeOut;
      this.tiempoParaReasignarAlerta = data.tiempoParaReasignarAlerta;
    }

  }
  public id: string | null = null;
  public usuarioActivosMaximos: number = 0;
  public tiempoEnvioSMSSeconds: number = 0;
  public monitorPausaTimeOut: number = 0;
  public confirmarTimeOut: number = 0;
  public tiempoParaReasignarAlerta: number = 0;
}
