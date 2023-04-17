import { Entity } from "../../../shared/domain/Entity";

export class MapaConfigEntity extends Entity {

  constructor(data: any = undefined) {
    super(data);

    if (data !== undefined && data !== null) {
      this.apiKey = data.apiKey;
      this.zoom = data.zoom;
      this.center = data.center;
    }

  }
  public apiKey: string | undefined | null = "";
  public zoom: number = 15;
  public center: { lat: number, lon: number } = { lat: 0, lon: 0 };

}
