import { AfterContentInit, ChangeDetectorRef, Directive, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { ColDef, ColGroupDef } from 'ag-grid-community';
import * as moment from 'moment';
import { Subscription } from 'rxjs';
import { AlertComponent } from '../alert/alert.component';
import { ApplicationCrud } from '../application/ApplicationBase';
import { ActionButtonData } from '../grid/action-buttons.component';
import { ClickedEvent, SortChange, SortItem } from '../grid/grid.component';
import { ModelsService } from '../guard/model';
import { Page, PageGeneric, PagingFilter, PagingOrder, ReturnData } from '../rest/rest.service';

export enum CrudMode {
  None,
  Search,
  Add,
  Edit,
}

export interface NoParamConstructor<T> {
  new(): T;
  new(data: any): T;
}

export interface SelectItem {
  id: number | string | undefined;
  label: string | undefined;
}

@Directive()
export abstract class CrudControllerComponent<T extends object> implements OnInit, OnDestroy, AfterContentInit {

  public saveMessage: string = "Se ha guadado exitosamente";

  public sort: Array<SortItem> = [];

  public page: PageGeneric<T> = new PageGeneric<T>();

  public columnDefs: Array<ColDef | ColGroupDef> | undefined;

  public formSearch: FormGroup | undefined;

  public formEdit: FormGroup | undefined;

  public invalidFields: any | null = null;

  public estados: Array<SelectItem> = [{ id: "A", label: "Activo" }, { id: "D", label: "Inactivo" }];

  public estadosTodos: Array<SelectItem> = [{ id: "T", label: "Todos" }, { id: "A", label: "Activo" }, { id: "D", label: "Inactivo" }];

  public mode = CrudMode.Search;

  public isSearching = false;

  public searchText: string = "";

  public item: T | undefined;

  public estadoFormEdit: boolean = true;

  public subs: Subscription | undefined;

  constructor(
    public router: Router,
    public activatedRoute: ActivatedRoute,
    public ref: ChangeDetectorRef,
    public formBuilder: FormBuilder,
    public dialog: MatDialog,
    public modelService: ModelsService,
    public layoutPath: string,
    public appCrud: ApplicationCrud<T>,
    public ctor: NoParamConstructor<T>,
    mode: CrudMode) {
    this.onChangeMode(mode);
  }

  public convertDateTime(item: any): string {
    try {
      return moment(item).format("DD/MM/YYYY HH:mm");
    } catch (e) {
      return "---";
    }
  }

  public convertDate(item: any): string {
    try {
      return moment(item).format("DD/MM/YYYY");
    } catch (e) {
      return "---";
    }
  }

  public convertTime(item: any): string {
    try {
      return moment(item).format("HH:mm");
    } catch (e) {
      return "---";
    }
  }

  public findElement(value: string | number, array: Array<SelectItem>): string {
    const find = array.find((item) => { return item.id === value; });
    return find?.label ?? "";
  }

  public getEstado(value: string | number): string {
    const find = this.estadosTodos.find((item) => { return item.id === value; });
    return find?.label ?? "";
  }

  public ColumnAction(template: Array<ActionButtonData> | undefined = undefined, width = 120): ColDef {
    template = template === undefined ? [{ icon: "edit", title: "Editar", action: "edit" }, { icon: "delete", title: "Borrar", action: "delete" }] as Array<ActionButtonData> : template;
    return {
      headerName: 'Acción',
      width: width,
      maxWidth: width,
      minWidth: width,
      suppressMenu: true,
      sortable: false,
      suppressMovable: true,
      suppressAutoSize: true,
      resizable: false,
      pinned: "right",
      cellRenderer: "actionButtons",
      cellRendererParams: { buttons: template }
    } as ColDef
  }

  public onGridRowClicked(action: ClickedEvent) {
    if (action !== undefined && action.action > "") {
      switch (action.action) {
        case "edit": {
          this.onEdit(action.data as T);
          break;
        }
        case "delete": {
          this.onDelete(action.data as T);
          break;
        }
        default: {
          alert(action.action);
          break;
        }
      }
    }
  }

  public onSortChanged(sortEvent: SortChange): void {
    this.sort = sortEvent.sort;
    this.onSearch();
  }

  public onClickSearch(searchText: string): void {
    this.page.pageIndex = 1;
    this.searchText = searchText;
    this.onSearch();
  }

  public showOverlay(): void {
    this.modelService.showHourglass = true
  }

  public hideOverlay(): void {
    this.modelService.showHourglass = false;
  }

  public onSearch(excecuteSearch = true): Page | null {

    this.page.filter = [];
    this.page.order = [];
    this.page.filter.push({ property: 'searchText', condition: 'X', value: this.searchText } as PagingFilter);
    if (this.onSearchBefore()) {

      if ((this.sort?.length ?? 0) > 0)
        this.sort.forEach((item) => {
          let prop = item.colId.substr(0, 1).toUpperCase() + item.colId.substring(1, item.colId.length);
          this.page.order.push({ property: prop, direction: item.sort === "desc" ? "-" : "+" } as PagingOrder);
        });

      const page: Page = new Page(this.page);
      if (excecuteSearch) {
        this.showOverlay();
        this.isSearching = true;

        setTimeout(() => {
          this.appCrud.Find(page).subscribe((r: ReturnData) => {
            if (!r.isError) {
              this.page = r.data as PageGeneric<T>;
            }
            this.hideOverlay();
            this.isSearching = false;
            this.onSearchAfter(!r.isError, r);
          });
        }, 500);
      }

      return page;
    }
    return null;
  }

  public onSearchBefore(): boolean {
    return true;
  }

  public onSearchAfter(isOk: boolean, r: ReturnData): void {
    if (!isOk) {
      const error = this.processDataError(r);
      this.dialog.open(AlertComponent, { data: { titulo: "Error", label: error, type: "error" }, panelClass: ['mo-alert'], autoFocus: false });
    }
  }

  public onSaveDialog(data: T, continueInEdition = false) {
    const diag = this.dialog.open(AlertComponent, { data: { titulo: "Guardar", label: "¿Está seguro de querer guardar este elemento?", type: "question" }, panelClass: ['mo-alert'], autoFocus: false });
    const subscrib = diag.afterClosed().subscribe(result => {
      subscrib.unsubscribe();
      if (result) {
        this.invalidFields = null;
        this.modelService.showHourglass = true;
        this.appCrud.Save(data).subscribe((r: ReturnData) => {
          if (!r.isError) {
            if (continueInEdition) {
              const selectItem = r.data as T;
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

  private onSaveInternal(continueInEdition = false): void {
    if (this.formEdit !== undefined) {
      if (this.formEdit.valid) {
        const data = this.formEdit.value;
        if (this.onSaveBefore(data)) {
          this.onSaveDialog(data, continueInEdition);
        }
      }
      else {
        Object.keys(this.formEdit.controls).forEach(key => {
          this.formEdit?.get(key)?.markAsDirty();
        });
      }
    }
  }

  public onSaveAndContinue(): void {
    this.onSaveInternal(true);
  }

  public onSave(): void {
    this.onSaveInternal(false);
  }

  public onSaveBefore(data: T): boolean {
    return true;
  }

  public onSaveAfter(isOk: boolean, r: ReturnData | null): void {
    if (isOk) {
      this.onCancel();
      return;
    }

    this.modelService.showHourglass = false;
    this.invalidFields = [];

    if (r !== null) {

      let error = this.processDataError(r) != "" ? this.processDataError(r) : "default-error-save-message";

      r = (r.data.error ?? r) as ReturnData;

      const array = (r?.data?.data ?? (r as any).error) as Array<any>;
      if (array !== undefined && array !== null && array.length > 0) {
        let invalidModel = false;
        array.forEach((item) => {
          if (item.memberNames !== undefined && item.memberNames !== null && item.memberNames.length > 0) {
            this.invalidFields[item.memberNames[0].toString().toLowerCase()] = (item.errorMessage);
            invalidModel = true;
          }
        });

        if (invalidModel && error === "default-error-save-message")
          error = "default-data-invalid";
      }
      this.openDialogError(error);
    }
  }

  public openDialogError(error: string) {
    this.dialog.open(AlertComponent, { data: { label: error, type: "error" }, panelClass: ['mo-alert'], autoFocus: false });
  }

  public openDialogOk(error: string) {
    this.dialog.open(AlertComponent, { data: { label: error, type: "info" }, panelClass: ['mo-alert'], autoFocus: false });
  }

  public onError(property: string): string | null {
    if (this.formEdit !== undefined && this.formEdit !== null) {
      let error = this.onErrorFormGroup(property, this.formEdit);
      if (error === undefined || error === null || error === "") {
        error = this.onErrorReuqest(property);
      }
      return error;
    }
    return null;
  }

  public onErrorReuqest(property: string) {
    if (this.invalidFields !== undefined && this.invalidFields !== null) {
      property = property.toLowerCase();
      if (this.invalidFields[property] !== undefined && this.invalidFields[property] !== null) {
        return this.invalidFields[property];
      }
    }
    return null;
  }

  public onErrorFormGroup(property: string, formGroup: FormGroup): string | null {
    try {
      let controler = formGroup?.get(property);
      if (controler?.invalid && (controler?.touched || controler?.dirty) && controler?.errors !== null) {
        let errors = ''
        Object.keys(controler?.errors).forEach(key => {
          //errors += `${property}_${key}<br/>`
          errors += `${key}<br/>`
        });
        return errors > '' ? errors : null;
      }
    }
    catch (e) { }
    return null;
  }

  public onDelete(data: T): void {
    if (this.onDeleteBefore(data)) {
      const dialog = this.dialog.open(AlertComponent, { data: { titulo: "Borrar", label: "¿Está seguro de querer borrar este elemento?", type: "warning" }, panelClass: ['mo-alert'], autoFocus: false });
      dialog.afterClosed().subscribe(result => {
        if (result) {
          this.modelService.showHourglass = true;
          this.appCrud.Delete(this.getId(data)).subscribe((r: ReturnData) => {
            this.onDeleteAfter(!r.isError, r);
          })
        }
      })
    }
  }

  public onDeleteBefore(data: T): boolean {
    return true;
  }

  public onDeleteAfter(isOk: boolean, r: ReturnData): void {
    if (isOk)
      this.onCancel(true);
    else {
      const error = this.processDataError(r);
      this.dialog.open(AlertComponent, { data: { titulo: "Error", label: error, type: "error" }, panelClass: ['mo-alert'], autoFocus: false });
    }
  }

  public setFormGroupByObject(fg: FormGroup, obj: object) {
    for (const [key, val] of Object.entries(obj)) {
      if (fg.controls[key] !== undefined) {
        fg.controls[key].setValue(val);
      }
      else
        console.log("not found key : " + key)
    }
  }

  public createFromEdit(data: T) {
    if (this.formEdit !== undefined) {
      this.formEdit.reset();
      const value = new this.ctor(data);
      this.item = value;
      this.setFormGroupByObject(this.formEdit, value);
    }
  }

  public onHabilitateEdit() {
    if (this.formEdit !== undefined) {
      this.estadoFormEdit = true;
      this.formEdit.enable();
    }
  }

  public onAdd(data: T | undefined = undefined): void {
    if (this.formEdit !== undefined) {
      this.formEdit.enable();
      if (data == undefined)
        this.router.navigate([this.layoutPath + "/new"]);
      if (data === undefined) {
        data = new this.ctor();
        this.modelService.showHourglass = false;
      }
      if (this.onAddBefore(data)) {
        this.createFromEdit(data);
        this.onAddAfter();
        this.ref.detectChanges();
      }
    }
  }

  public onAddBefore(data: T): boolean {
    return true;
  }

  public onAddAfter(): void {
    this.onChangeMode(CrudMode.Add);
    this.modelService.showHourglass = false;
  }

  public abstract getId(data: T): any;
  public abstract getLabel(data: T): any;

  public onEdit(data: T): void {
    if (this.onEditBefore(data)) {
      if (this.getId(data) !== undefined && this.getId(data) !== null && this.router.url.indexOf(this.layoutPath + "/" + this.getId(data)) === -1) {
        this.router.navigate([this.layoutPath + "/" + this.getId(data)]);
        return;
      }
      this.createFromEdit(data);
      //if (this.formEdit !== undefined)
      //  this.formEdit.disable();
      this.estadoFormEdit = true;
      this.onEditAfter();
      this.ref.detectChanges();
    }
  }

  public onEditBefore(data: T): boolean {
    return true;
  }

  public onEditAfter(): void {
    //if (this.formEdit !== undefined)
    //  this.formEdit.disable();
    this.onChangeMode(CrudMode.Edit);
  }

  public onChangeMode(mode: CrudMode): void {
    if (this.onChangeModeBefore(mode)) {
      this.mode = mode;
      this.onChangeModeAfter();
    }
  }

  public onChangeModeBefore(mode: CrudMode): boolean {
    return true;
  }

  public onChangeModeAfter(): void {
    //Funcion para implementar del lado del controller para detectar el cambio de modo
  }

  public onCancelButton(findObject = false): void {
    const diag = this.dialog.open(AlertComponent, { data: { label: "¿Está seguro de querer cancelar?", type: "warning" }, panelClass: ['mo-alert'], autoFocus: false });
    diag.afterClosed().subscribe(result => {
      if (result)
        if (this.onCancelBefore()) {
          this.onCancelAfter(findObject);
        }
    });
  }

  public onCancel(findObject = false): void {
    if (this.onCancelBefore()) {
      this.onCancelAfter(findObject);
    }
  }

  public onCancelBefore(): boolean {
    return true;
  }

  public onCancelAfter(findObject = false): void {
    this.invalidFields = null;
    this.router.navigate([this.layoutPath + "/find"]);
    this.onChangeMode(CrudMode.Search);
    if (findObject || this.page.recordCount == 0) {
      this.searchText = "";
      this.onSearch();
    }
    else
      setTimeout(() => {
        this.hideOverlay();
        this.modelService.showHourglass = false;
      }, 500);
  }

  public isModeSearch(): boolean {
    return this.mode === CrudMode.Search;
  }

  public isModeEdit(): boolean {
    return this.mode === CrudMode.Edit || this.mode === CrudMode.Add;
  }

  public isModeAdd(): boolean {
    return this.mode === CrudMode.Add;
  }

  public processDataError(r: ReturnData): string {
    if (r.data.error !== undefined && r.data.error !== null && r.data.error.data !== undefined && r.data.error.data !== null)
      return r.data.error.data.message;
    else if (typeof (r.data) === "string")
      return r.data.toString();
    return "";
  }

  public isValid(field: string) {
    if (field !== null) {
      if (this.invalidFields !== null) {
        if (this.invalidFields[field.toLowerCase()] !== undefined)
          return false;
      }
      if (this.formEdit !== undefined) {
        const _field = this.formEdit.get(field);
        if (_field !== null)
          return !(_field.invalid && (_field.dirty || _field.touched));
      }
    }
    return true;
  }

  public validText(field: string, errors = [["required", "requerido"]]) {

    let result = "";
    var fieldL = field.toLowerCase();
    if (this.invalidFields !== null && this.invalidFields[fieldL] !== undefined) {
      result = `${this.invalidFields[fieldL]}<br/>`;
    }

    if (this.formEdit !== undefined) {
      const _field = this.formEdit.get(field);
      if (_field !== null && _field.invalid && (_field.dirty || _field.touched)) {
        errors.forEach(error => {
          if (_field.getError(error[0])) {
            result = `${result}${error[1]}<br/>`;
          }
        });
      }
    }

    return result;
  }

  public firstSearch() {
    if (this.mode === CrudMode.Search)
      this.onClickSearch("");
  }

  ngOnInit(): void {
    this.subs = this.activatedRoute.params.subscribe((param: any) => {
      if (param.id === "new") {
        this.onAdd();
        return;
      }
      if (param.id === "find") {
        this.onCancel(true);
        return;
      }
      else {
        let id: any = param.id;
        if (id !== undefined && id !== null) {
          this.mode = CrudMode.None;
          this.modelService.showHourglass = true;

          this.appCrud.Edit(id).subscribe((r: ReturnData) => {
            if (!r.isError) {
              this.modelService.showHourglass = false;
              this.onEdit(new this.ctor(r.data));
            }
            else {
              this.onCancel(true);
            }
          })
        }
      }
    });
  }

  ngAfterContentInit(): void {
    setTimeout(() => {
      this.firstSearch();
    }, 0);
  }

  ngOnDestroy(): void {
    if (this.subs !== undefined)
      this.subs.unsubscribe();
  }
}
