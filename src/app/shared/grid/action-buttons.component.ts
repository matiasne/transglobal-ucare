import { Component, PipeTransform, Pipe } from '@angular/core';
import { ICellEditorAngularComp } from 'ag-grid-angular';
import { IAfterGuiAttachedParams, ICellEditorParams } from 'ag-grid-community';

@Component({
  selector: 'mo-action-buttons',
  template: `<mat-icon fontSet="material-icons-outlined" *ngFor="let item of icons | filterButtons:params" matTooltip="{{item.title}}" matTooltipClass="mo-tooltip" data-action-type="{{item.action}}" style="cursor: pointer;{{(item.color>''?('color:' + item.color +';'):'')}}{{(item.style>''?item.style:'')}}" [class]="item.class">{{item.icon}}</mat-icon>`,
  styles: ['mat-icon{margin-right:10px;color: #8D99AE;} mat-icon:last-child {margin-right:0px;}']
})
export class AgActionButtonsComponent implements ICellEditorAngularComp {

  public params: ICellEditorParams = {} as ICellEditorParams;
  public icons: Array<ActionButtonData> = [];

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
    this.icons = (params as any).buttons as Array<ActionButtonData>;
  }

  afterGuiAttached?(params?: IAfterGuiAttachedParams): void {
  }

}

@Pipe({
  name: 'filterButtons',
  pure: false
})
export class FilterButtons implements PipeTransform {
  transform(items: any[], filter: ICellEditorParams): any {
    if (!items || !filter) {
      return items;
    }
    // filter items array, items which match and return true will be
    // kept, false will be filtered out
    return items.filter(item => this.isShow(item, filter));
  }

  isShow(item: ActionButtonData, params: ICellEditorParams): boolean {
    if (item.isShow !== null && item.isShow !== undefined) {
      return item.isShow(item, params);
    }
    return true;
  }
}

export class ActionButtonData {
  public icon: string = "";
  public title: string = "";
  public action: string = "";
  public color: string = "";
  public class: string = "";
  public style: string = "";
  public isShow: (item: ActionButtonData, param: ICellEditorParams) => boolean = () => true;
}
