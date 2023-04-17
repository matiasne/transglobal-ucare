import { Component, DoCheck, EventEmitter, HostListener, Input, IterableDiffers, OnInit, Output } from '@angular/core';
import { ColDef, ColGroupDef, ColumnApi, GridApi, GridOptions, SortChangedEvent } from 'ag-grid-community';
import { AgActionButtonsComponent } from './action-buttons.component';
import { ImageFormatterComponent } from './ImageFormatterComponent';

@Component({
  selector: 'gq-grid',
  templateUrl: './grid.component.html',
  styleUrls: ['./grid.component.scss'],
})
export class GridComponent implements OnInit, DoCheck {

  private iterableDiffer: any;

  public sort: { colId: string | null | undefined, sort: string | null | undefined, sortIndex?: number | null; }[] = [];
  public searchValue: any = null;
  private _gridOptions: GridOptions | undefined;

  @Input()
  public get gridOptions(): GridOptions | undefined { return this._gridOptions; }
  public set gridOptions(value: GridOptions | undefined) { this._gridOptions = value; }

  @Input() public noRowsText = "No se encontraron registros";

  private _columnDefs: Array<ColDef | ColGroupDef> = [];
  @Input()
  public get columnDefs(): Array<ColDef | ColGroupDef> { return this._columnDefs; }
  public set columnDefs(value: Array<ColDef | ColGroupDef>) { this._columnDefs = value; this.setColumnns(); }

  private _data: Array<object> = [];
  @Input()
  public get data(): Array<object> { return this._data; }
  public set data(value: Array<object>) { this._data = value; this.setDataRow(); }

  private _showLoadingOverlay = false;
  @Input()
  public get showLoadingOverlay(): boolean { return this._showLoadingOverlay; }
  public set showLoadingOverlay(value: boolean) { this._showLoadingOverlay = value; this.setShowLoadingOverlay(); }

  private _rowClassRules: ((params: any) => (string | string[])) | null = null;
  @Input()
  public get rowClassRules(): ((params: any) => (string | string[])) | null { return this._rowClassRules; }
  public set rowClassRules(value: ((params: any) => (string | string[])) | null) { this._rowClassRules = value; }

  @Output() public onReady: EventEmitter<ApiGrid> = new EventEmitter();
  @Output() public onSortChanged: EventEmitter<SortChange> = new EventEmitter();
  @Output() public onRowClicked: EventEmitter<ClickedEvent> = new EventEmitter();

  private api: GridApi | null = null;
  private columnApi: ColumnApi | null = null;

  public constructor(private iterableDiffers: IterableDiffers) {
    this.iterableDiffer = iterableDiffers.find([]).create();
  }

  ngDoCheck() {
    let changes = this.iterableDiffer.diff(this.data);
    if (changes) {
      this.setDataRow();
    }
  }

  public onRowClickedInternal(e: any) {
    if (e.event.target !== undefined) {
      const data = e.data;
      const actionType = e.event.target.getAttribute("data-action-type") === null ? e.event.target["action-type"] : e.event.target.getAttribute("data-action-type");
      this.onRowClicked.emit({ action: actionType, data: data } as ClickedEvent);
      if (this.api !== null)
        this.api.redrawRows();
    }
  }

  public onReadyGrid(params: any) {
    this.api = params.api;
    this.columnApi = params.columnApi;
    this.setColumnns();
    this.setDataRow();
    this.setShowLoadingOverlay();

    this.onReady.emit({ api: this.api, columnApi: this.columnApi } as ApiGrid);
    if (this.api !== null)
      this.api.sizeColumnsToFit();
  }


  private setColumnns() {
    if (this.api !== null) {
      this.api.setColumnDefs(this._columnDefs);
    }
  }

  private setDataRow() {
    if (this.api !== null) {
      this.api.setRowData(this._data);
      this.api.sizeColumnsToFit();
    }
  }

  private setShowLoadingOverlay() {
    if (this.api !== null) {
      if (this._showLoadingOverlay)
        this.api.showLoadingOverlay();
      else
        this.api.hideOverlay();
    }
  }
  public quickSearch($event: any) {
    if (this.api !== null)
      this.api.setQuickFilter($event.target.value)
  }

  ngOnInit(): void {
    if (this._gridOptions === undefined) {
      this.gridOptions = {
        rowData: this._data,
        rowHeight: 32,
        getRowClass: (param) => {
          return this.rowClassRules instanceof Function ? this.rowClassRules(param) : "";
        },
        columnDefs: this._columnDefs,
        context: {
          componentParent: this
        },
        frameworkComponents: {
          actionButtons: AgActionButtonsComponent,
          imagen: ImageFormatterComponent
        },
        onSortChanged: (e) => {

          this.sort = [];

          for (let i = 0; i < e.columnApi.getColumnState().length; i++) {
            if (e.columnApi.getColumnState()[i].sort !== undefined && e.columnApi.getColumnState()[i].sort !== null)
              this.sort.push({ colId: e.columnApi.getColumnState()[i].colId, sort: e.columnApi.getColumnState()[i].sort, sortIndex: e.columnApi.getColumnState()[i].sortIndex });
          }

          this.sort = this.sort.sort((a, b) => { return (a.sortIndex ?? 0) - (b.sortIndex ?? 0) }); // Ordena el arreglo

          this.onSortChanged.emit({ e: e, api: this.api, columnApi: this.columnApi, sort: this.sort } as SortChange);
        },
        onRowClicked: (e) => { this.onRowClickedInternal(e); }
      } as GridOptions;
    }
    this.onWindowsResize();
  }

  private timeout: any = null;
  @HostListener('window:resize', ['$event'])
  private onWindowsResize() {
    if (this.api !== null) {
      if (this.timeout !== null) {
        clearTimeout(this.timeout);
      }
      this.timeout = setTimeout(() => {
        if (this.api !== undefined && this.api !== null) {
          this.api.sizeColumnsToFit();
        }
        this.timeout = null;
      }, 500);
    }
  }
}

export interface ClickedEvent {
  action: string;
  data: any;
}

export interface ApiGrid {
  api: GridApi;
  columnApi: ColumnApi;
}

export interface SortChange extends ApiGrid {
  e: SortChangedEvent;
  sort: Array<SortItem>;
}

export interface SortItem {
  colId: string;
  sort: string;
}
