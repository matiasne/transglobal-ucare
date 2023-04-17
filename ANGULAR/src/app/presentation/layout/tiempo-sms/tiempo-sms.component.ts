import { ChangeDetectorRef, Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { ConfigApp } from '../../../core/config/application/config.app';
import { ConfigEntity } from '../../../core/config/domain/config.entity';
import { AlertComponent } from '../../../shared/alert/alert.component';
import { Constants } from '../../../shared/Constants';
import { CrudControllerComponent, CrudMode } from '../../../shared/controller/controller.service';
import { ModelsService } from '../../../shared/guard/model';
import { Page, ReturnData } from '../../../shared/rest/rest.service';

@Component({
  selector: 'app-tiempo-sms',
  templateUrl: './tiempo-sms.component.html',
  styleUrls: ['./tiempo-sms.component.scss']
})

export class TiempoSMSComponent extends CrudControllerComponent<ConfigEntity> {

  public override formEdit: FormGroup | undefined = this.formBuilder.group(
    {
      id: [],
      tiempoEnvioSMSSeconds: [0, Validators.required],
      monitorPausaTimeOut: [0, Validators.required],
      confirmarTimeOut: [0, Validators.required],
      tiempoParaReasignarAlerta: [0, Validators.required],
    });

  public override onSearch(excecuteSearch = true): Page | null {
    this.onEdit(new ConfigEntity({ id: "1" }));
    return null;
  }

  public getId(data: ConfigEntity) {
    return 1;
  }

  public getLabel(data: ConfigEntity) {
    return data.usuarioActivosMaximos;
  }

  public override onSaveDialog(data: ConfigEntity, continueInEdition = false) {
    const diag = this.dialog.open(AlertComponent, { data: { titulo: "Guardar", label: "¿Está seguro de querer guardar este elemento?", type: "question" }, panelClass: ['mo-alert'], autoFocus: false });
    const subscrib = diag.afterClosed().subscribe(result => {
      subscrib.unsubscribe();
      if (result) {
        this.invalidFields = null;
        this.modelService.showHourglass = true;
        this.appCrud.SaveTiempoEnvioSMSSeconds(data).subscribe((r: ReturnData) => {
          if (!r.isError) {
            if (continueInEdition) {
              const selectItem = r.data as ConfigEntity;
              this.onEdit(selectItem);
            }
            else {
              this.onSaveAfter(!r.isError, r);
              this.onSearch();
            }
          }
          else
            this.onSaveAfter(!r.isError, r);
          this.modelService.showHourglass = false;
        });
      }
    });
  }

  constructor(
    public override router: Router,
    public override activatedRoute: ActivatedRoute,
    public override ref: ChangeDetectorRef,
    public override formBuilder: FormBuilder,
    public override dialog: MatDialog,
    public override modelService: ModelsService,
    public override appCrud: ConfigApp,
  ) {
    super(router, activatedRoute, ref, formBuilder, dialog, modelService, Constants.LAYOUT_TEMPO_SMS, appCrud, ConfigEntity, CrudMode.Search);
  }
}
