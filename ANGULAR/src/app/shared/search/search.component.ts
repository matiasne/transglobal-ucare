import { Component, Directive, EventEmitter, Input, Output, TemplateRef } from "@angular/core";
import { FormBuilder, FormGroup } from '@angular/forms';

@Directive({ selector: '[mo-add-button-search]' })
export class MoAddButtonTemplateDirective {
  constructor(public template: TemplateRef<any>) { }
}

@Component({
  selector: 'mo-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.scss'],
})
export class SearchComponent {
  public searchOK: boolean = false;

  @Input() public placeholder = "Buscar";

  @Input() public showAdd = true;
  @Input() public showExport = false;
  @Input() public showImport = false;
  @Input() public showSearch = true;
  @Input() public isOpen = true;
  @Input() public addText = "";
  @Input() public filterText = "";
  @Input() public minCharactersToSearch = 3;

  @Output() onClickSearch: EventEmitter<string> = new EventEmitter();
  @Output() onClickExport: EventEmitter<object> = new EventEmitter();
  @Output() onClickImport: EventEmitter<File> = new EventEmitter();
  @Output() onClickAdd: EventEmitter<object> = new EventEmitter();
  @Output() onClickOpenCloseFilter: EventEmitter<boolean> = new EventEmitter();

  @Input() moAddButtonTemplate: TemplateRef<null> | undefined;
  public formSearch: FormGroup | undefined;

  private timeOut: any;

  constructor(public formBuilder: FormBuilder) {
    this.formSearch = this.formBuilder.group({
      searchText: [''],
    });
  }
  public onKeyUp() {
    clearTimeout(this.timeOut);
    if (this.formSearch !== undefined)
      if (this.minCharactersToSearch > this.formSearch.value.searchText.length) {
        if (this.searchOK) {
          this.searchOK = false;
          this.timeOut = setTimeout(this.onSearchAll.bind(this), 50);
        }
        return
      }
    this.searchOK = true;
    this.timeOut = setTimeout(this.onSearch.bind(this), 500);

  }

  public onOpenClose(): void {
    this.isOpen = !this.isOpen;
    this.onClickOpenCloseFilter.emit(this.isOpen);
  }

  public onSearch(): void {
    if (this.formSearch !== undefined)
      this.onClickSearch.emit(this.formSearch.value.searchText);
  }
  public onSearchAll(): void {
    if (this.formSearch !== undefined) {
      this.formSearch.setValue({ searchText: "" });
      this.onClickSearch.emit(this.formSearch.value.searchText);
    }
  }

  public onExport(): void {
    this.onClickExport.emit();
  }

  public onImport(event: any): void {
    const fileTypes = ['xls', 'xlsx'];

    if (event !== undefined && event !== null) {
      if (event.target.files.length == 1) {
        this.onClickImport.emit(event.target.files[0]);
      }
    }
    event.target.value = null;
  }

  public onCleanSearch() {
    this.onSearchAll();
  }

  public onAdd(): void {
    this.onClickAdd.emit();
  }
}
