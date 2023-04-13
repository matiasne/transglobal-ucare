import { ChangeDetectorRef, Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { MapaConfigApp } from '../../../core/mapa-config/application/mapa-config.app';
import { MapaConfigEntity } from '../../../core/mapa-config/domain/mapa-config.entity';
import { Constants } from '../../../shared/Constants';
import { CrudControllerComponent, CrudMode } from '../../../shared/controller/controller.service';
import { ModelsService } from '../../../shared/guard/model';
import { Page } from '../../../shared/rest/rest.service';

@Component({
  selector: 'app-mapa-config',
  templateUrl: './mapa-config.component.html',
  styleUrls: ['./mapa-config.component.scss']
})

export class MapaConfigComponent extends CrudControllerComponent<MapaConfigEntity> {

  public override formEdit: FormGroup | undefined = this.formBuilder.group(
    {
      zoom: [15, [Validators.required, Validators.max(16), Validators.min(9)]],
      lat: [0, Validators.required],
      lon: [0, Validators.required],
    });

  public override onSearch(excecuteSearch = true): Page | null {
    this.onEdit(new MapaConfigEntity({ id: "1" }));
    return null;
  }
  public override createFromEdit(data: MapaConfigEntity) {
    if (this.formEdit !== undefined) {
      this.formEdit.reset();
      const value = new this.ctor(data);
      this.item = value;
      this.formEdit.controls["zoom"].setValue(value?.zoom ?? 15);
      this.formEdit.controls["lat"].setValue(value?.center?.lat ?? 0);
      this.formEdit.controls["lon"].setValue(value?.center?.lon ?? 0);
    }
  }

  public override onSaveBefore(data: MapaConfigEntity) {

    if (data.zoom < 9 || data.zoom > 16) {
      return false;
    }

    data.center = { lat: this.formEdit!.controls["lat"].value, lon: this.formEdit!.controls["lon"].value };
    return true;
  }


  public getId(data: MapaConfigEntity) {
    return 1;
  }

  public getLabel(data: MapaConfigEntity) {
    return "Mapa Config";
  }

  constructor(
    public override router: Router,
    public override activatedRoute: ActivatedRoute,
    public override ref: ChangeDetectorRef,
    public override formBuilder: FormBuilder,
    public override dialog: MatDialog,
    public override modelService: ModelsService,
    public override appCrud: MapaConfigApp,
  ) {
    super(router, activatedRoute, ref, formBuilder, dialog, modelService, Constants.LAYOUT_MAPA_CONFIG, appCrud, MapaConfigEntity, CrudMode.Search);
  }
}
