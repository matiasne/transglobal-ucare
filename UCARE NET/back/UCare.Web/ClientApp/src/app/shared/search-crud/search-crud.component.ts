import { AfterViewInit, Component, ContentChild, Directive, EventEmitter, Input, Output, TemplateRef } from "@angular/core";
import { MatPaginatorIntl } from '@angular/material/paginator';
import { ColDef, ColGroupDef } from "ag-grid-community";
import { ClickedEvent, SortChange } from "../grid/grid.component";
import { Page } from "../rest/rest.service";
import { MoAddButtonTemplateDirective } from '../search/search.component';

@Directive({ selector: '[gq-content-search]' })
export class GQContentSearchTemplateDirective {
  constructor(public template: TemplateRef<any>) { }
}

@Component({
  selector: 'gq-search-crud',
  templateUrl: './search-crud.component.html',
  styleUrls: ['./search-crud.component.scss'],
})

export class SearchCrudComponent implements AfterViewInit {
  @Input() public windowsTitle = "";
  @Input() public descripcion = "";
  @Input() public classStyle = "";
  @Input() public showExport = false;
  @Input() public showImport = false;
  @Input() public showAdd = true;
  @Input() public showSearch = true;
  @Input() public addText = "";
  @Input() public filterText = "";
  @Input() public showPaging = true;

  @Input() public minCharactersToSearch = 3;

  @Input() public page: Page | undefined;

  @Input() public columnDefs: Array<ColDef | ColGroupDef> | undefined;
  @Input() public showLoadingOverlay: boolean = false;

  @ContentChild(GQContentSearchTemplateDirective, { read: TemplateRef, static: false }) gqContentSearchTemplate: TemplateRef<any> | undefined;;
  @ContentChild(MoAddButtonTemplateDirective, { read: TemplateRef, static: false }) MoAddButtonTemplate: TemplateRef<any> | undefined;

  @Output() onClickSearch: EventEmitter<string> = new EventEmitter();
  @Output() onSearch: EventEmitter<object> = new EventEmitter();
  @Output() onClickExport: EventEmitter<object> = new EventEmitter();
  @Output() onClickImport: EventEmitter<File> = new EventEmitter();
  @Output() onClickAdd: EventEmitter<object> = new EventEmitter();

  @Output() public onItemClicked: EventEmitter<ClickedEvent> = new EventEmitter();
  @Output() public onSortChanged: EventEmitter<SortChange> = new EventEmitter();

  public isOpenFilter: boolean = false;

  ngAfterViewInit() {
    console.log()
  }
  public onAdd(): void {
    this.onClickAdd.emit();
  }

  public onExport(): void {
    this.onClickExport.emit();
  }

  public onImport(file: File): void {
    this.onClickImport.emit(file);
  }

  public _onRowClicked(params: any) {
    this.onItemClicked.emit(params);
  }

  public _onSortChanged(params: any) {
    this.onSortChanged.emit(params);
  }

  public onPage($event: { pageIndex: number; pageSize: number; }) {
    if (this.page === undefined) {
      this.page = new Page();
    }

    this.page.pageIndex = $event.pageIndex + 1;
    this.page.pageSize = $event.pageSize;
    this._onSearch();
  }

  public _onClickSearch(searchText: string | undefined): void {
    this.onClickSearch.emit(searchText);
  }

  public _onSearch(): void {
    this.onSearch.emit(this.page);
  }

  public onOpenCloseFilter(event: boolean) {
    this.isOpenFilter = event;
  }
}

export class PaginatorI18n {

  getPaginatorIntl(): MatPaginatorIntl {
    const paginatorIntl = new MatPaginatorIntl();
    paginatorIntl.itemsPerPageLabel = "Items"; // this.translate.transform('default-paging-itemsPerPageLabel');
    paginatorIntl.nextPageLabel = "Siguiente"; //this.translate.transform('default-paging-nextPageLabel');
    paginatorIntl.previousPageLabel = "Anterior"; //this.translate.transform('default-paging-previousPageLabel');
    paginatorIntl.firstPageLabel = "Primer"; //this.translate.transform('default-paging-firstPageLabel');
    paginatorIntl.lastPageLabel = "Ulimo"; //this.translate.transform('default-paging-lastPageLabel');
    paginatorIntl.getRangeLabel = this.getRangeLabel.bind(this);
    return paginatorIntl;
  }

  private getRangeLabel(page: number, pageSize: number, length: number): string {
    if (length === 0 || pageSize === 0 || length <= pageSize) {
      return `no hay registros ${length}`; //this.translate.transform('default-paging-getRangeVacioLabel', { length });
    }
    length = Math.max(length, 0);
    const startIndex = page * pageSize;
    // If the start index exceeds the list length, do not try and fix the end index to the end.
    const endIndex = startIndex < length ? Math.min(startIndex + pageSize, length) : startIndex + pageSize;
    return `pagina ${startIndex + 1} de ${endIndex} registros ${length}`;//this.translate.transform('default-paging-getRangeLlenoLabel', { startIndex: startIndex + 1, endIndex, length });
  }
}

