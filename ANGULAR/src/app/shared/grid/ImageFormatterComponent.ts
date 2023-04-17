import { Component } from "@angular/core";
import { ICellEditorAngularComp } from 'ag-grid-angular';
import { IAfterGuiAttachedParams, ICellEditorParams } from 'ag-grid-community';

@Component({
  selector: 'app-image-formatter-cell',
  template: `<img alt="img" border="0" style="width: 30px; height: 30px; border-radius: 50%"  (error)="onError($event)" src=\"{{ params.value }}\">`
})

export class ImageFormatterComponent implements ICellEditorAngularComp {
  public params: ICellEditorParams = {} as ICellEditorParams;
  public urlImageError: string = 'assets/images/noimage.jpg';


  onError(event: any) {
    event.target.src = this.urlImageError;
  }

  getValue() {
    return null;
  }
  isPopup?(): boolean {
    return false;
  }
  getPopupPosition?(): string {
    return "";
  }
  isCancelBeforeStart?(): boolean {
    return true;
  }
  isCancelAfterEnd?(): boolean {
    return true;
  }
  focusIn?(): void {
  }
  focusOut?(): void {
  }
  getFrameworkComponentInstance?() {
  }
  agInit(params: ICellEditorParams): void {
    this.params = params;
  }
  afterGuiAttached?(params?: IAfterGuiAttachedParams): void {
  }
}


