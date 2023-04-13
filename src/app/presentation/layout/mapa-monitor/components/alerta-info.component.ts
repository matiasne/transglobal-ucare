import { Component, Input } from '@angular/core';
import * as moment from 'moment';
import { AlertasEntity } from '../../../../core/alertas/domain/alertas.entity';
import { SelectItem } from '../../../../shared/controller/controller.service';

@Component({
  selector: 'alerta-info',
  templateUrl: './alerta-info.component.html',
  styleUrls: ['./alerta-info.component.scss']
})
export class AlertaInfoComponent {

  @Input()
  public data: AlertasEntity | null = null;

  public sexosAfiliado: Array<SelectItem> = [{ id: "M", label: "Masculino" }, { id: "F", label: "Femenino" }, { id: "O", label: "Otro" }];

  public getSexo(sexo: string) {
    return this.sexosAfiliado.find((item) => {
      return item.id == sexo.substr(0, 1).toUpperCase();
    })?.label ?? "";
  }

  public ago(fecha: any) {
    return moment(fecha).fromNow()
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

}
