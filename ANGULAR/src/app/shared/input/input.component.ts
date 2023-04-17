import { DecimalPipe } from "@angular/common";
import { AfterViewInit, Component, ElementRef, EventEmitter, forwardRef, Inject, Input, Optional, Output, Renderer2, ViewChild } from "@angular/core";
import { COMPOSITION_BUFFER_MODE, ControlValueAccessor, NG_VALUE_ACCESSOR } from "@angular/forms";
import { MAT_MOMENT_DATE_ADAPTER_OPTIONS, MomentDateAdapter } from '@angular/material-moment-adapter';
import { MatAutocompleteSelectedEvent } from "@angular/material/autocomplete";
import { DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE } from '@angular/material/core';
import { MatDatepicker, MatDatepickerInputEvent } from '@angular/material/datepicker';
import { MatSelect } from "@angular/material/select";
import * as moment from 'moment';
import { NgxMaterialTimepickerComponent } from 'ngx-material-timepicker';

export const MY_FORMATS = {
  parse: {
    dateInput: 'DD/MM/YYYY',
  },
  display: {
    dateInput: 'DD/MM/YYYY',
    monthYearLabel: 'MMM YYYY',
    dateA11yLabel: 'LL',
    monthYearA11yLabel: 'MMMM YYYY',
  },
};

@Component({
  selector: 'gq-input',
  templateUrl: './input.component.html',
  styleUrls: ['./input.component.scss'],
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => InputComponent),
    multi: true
  }, {
    provide: DateAdapter,
    useClass: MomentDateAdapter,
    deps: [MAT_DATE_LOCALE, MAT_MOMENT_DATE_ADAPTER_OPTIONS]
  },
  { provide: MAT_DATE_FORMATS, useValue: MY_FORMATS },
  ]
})
export class InputComponent implements AfterViewInit, ControlValueAccessor {
  private valueFormat: any = null;

  @Output() onEnter: EventEmitter<object> = new EventEmitter();
  @Output() onFocus: EventEmitter<object> = new EventEmitter();

  @Output() change: EventEmitter<object> = new EventEmitter();
  @Output() clickIcon: EventEmitter<object> = new EventEmitter();

  @Input() public selectedValue = "";
  @Input() public label = "";
  @Input() public placeholder = "";
  @Input() public hint = "";
  @Input() public errorText: string | null = null;
  @Input() public errorShow = false;
  @Input() public disabled = false;
  @Input() public type: "text" | "number" | "select" | "date" | "time" | "password" | "textarea" | "auto" | "select-multi" = "text";
  @Input() public data: any;
  @Input() public dataLabel = "label";
  @Input() public dataValue = "id";
  @Input() public dataIcon = "icon";
  @Input() public prefix = "";
  @Input() public suffix = "";
  @Input() public readonly = false;
  @Input() public rows = 5;
  @Input() public icon = "";
  @Input() public isRequired = false;
  @Input() public showTimePickerIcon = false;
  @Input() public autoCompleteInput = "off"

  private _numberFormat = "1.0-4";
  @Input()
  public get numberFormat(): string {
    return this._numberFormat;
  }
  public set numberFormat(value: string) {
    this._numberFormat = value;
    try {
      const sp = value.split("-");
      if (sp.length > 1) {
        if (parseInt(sp[1]) > 0) {
          this.numberType = "decimal";
          return;
        }
      }
    }
    catch (error) { }
    this.numberType = "number";
  }
  private numberType: string | "number" | "decimal" = "decimal";

  private writeValueNoSet: any;
  private _composing = false;

  onChange: ((_: any) => void) | undefined;
  onTouched: (() => void) | undefined;

  @ViewChild('moInput', { static: false }) private moInput: ElementRef<HTMLInputElement> | undefined;
  @ViewChild('moMatSelect', { static: false }) private moMatSelect: MatSelect | undefined;
  @ViewChild('timePicker', { static: false }) private moNgxTime: NgxMaterialTimepickerComponent | undefined;
  @ViewChild('datePicker', { static: false }) private matDatePicker: MatDatepicker<any> | undefined;
  constructor(
    private _renderer: Renderer2,
    private _decimalPipe: DecimalPipe,
    private _adapter: DateAdapter<any>,
    @Optional() @Inject(COMPOSITION_BUFFER_MODE) private _compositionMode: boolean) {
    _adapter.setLocale('es')
  }

  public callOnTouched() {
    if (this.onTouched !== undefined)
      this.onTouched();
  }

  public onClickIcon() {
    this.clickIcon.emit(this);
  }

  public getLabel(item: any): string {
    if (this.dataLabel === "") return item.toString();
    else return item[this.dataLabel];
  }

  public getValue(item: any): object {
    if (this.dataValue === "") return item;
    else return item[this.dataValue];
  }

  public getIcon(item: any): object {
    if (this.dataIcon === "") return item;
    else return item[this.dataIcon];
  }

  public getValueFormat() {
    return this.valueFormat;
  }

  public cleanValue() {
    let value = this.writeValueFormat('');
    this.writeValue(value);
    if (this.onChange !== undefined)
      this.onChange(value);
    this.change.emit();
  }

  private getElement() {
    if (this.moMatSelect !== undefined && this.moMatSelect !== null)
      return this.moMatSelect;
    if (this.moInput !== undefined && this.moInput !== null)
      return this.moInput.nativeElement;

    return null;
  }

  private writeValueFormat(value: any) {
    switch (this.type) {
      case "time": {
        const h = value.split(":");
        value = new Date(0, 0, 0, h[0], h[1]);
        break;
      }
      case "date":
        {
          value = moment(value).toDate();
          break;
        }
      //case "select":
      //  {
      //    value = (value as MatSelectChange).value;
      //    break;
      //  }
      case "number":
        {
          value = (value === null || value === undefined) ? '' : value;
          const valwithoutformat = this._decimalPipe.transform(value, this.numberFormat) ?? "";
          const valorFormat = this.numberRemoveFormat(valwithoutformat);
          value = parseFloat(valorFormat);
        }
        break;
    }
    this.valueFormat = value;
    return value;
  }

  private numberRemoveFormat(value: string) {
    if (value !== null)
      value = value.replace(/,/gi, "");
    return value;
  }

  writeValue(value: any): void {
    try {

      if (this.getElement() === null) {
        this.writeValueNoSet = value;
        return;
      }

      this.writeValueNoSet = undefined;
      const normalizedValue = (value === null || value === undefined) ? '' : value;

      switch (this.type) {
        case "time":
          {
            if (this.moNgxTime != undefined)
              this.moNgxTime.timeSet.emit(moment(value).format("HH:mm"));
            else
              this._renderer.setProperty(this.getElement(), 'value', " ");
            break;
          }
        case "date":
          {
            setTimeout(() => {

              if (!value) {
                //const val = moment(value == ('' || null) ? undefined : value).format("DD/MM/YYYY");
                this.matDatePicker?.select(null);
                this._renderer.setProperty(this.getElement(), 'value', "");
              }
              else {
                this.matDatePicker?.select(moment(value));
                this._renderer.setProperty(this.getElement(), 'value', moment(value).format("DD/MM/YYYY"));
              }
            }, 200);
            break;
          }
        case "select": {
          if (this.moMatSelect !== undefined) {
            const result = this.moMatSelect.options.find((item, index, array) => {
              return item.value === value;
            });
            if (result !== undefined && result !== null)
              result.select();
            return;
          }
        }
          break;
        case "number": {
          try {
            const valwithoutformat = this._decimalPipe.transform(normalizedValue, this.numberFormat) ?? "";
            const valorFormat = this.numberRemoveFormat(valwithoutformat);
            this._renderer.setProperty(this.getElement(), 'value', valorFormat);
            value = valorFormat;
          }
          catch (error) {
            this._renderer.setProperty(this.getElement(), 'value', normalizedValue);
          }
        }
          break;
        default: {
          this._renderer.setProperty(this.getElement(), 'value', normalizedValue);
        }
      }
    } catch (e) {
      console.log(e);
    }
  }

  onAutoSelect(event: MatAutocompleteSelectedEvent) {
    this._handleInput(event.option.value);
  }

  onChangeDate(origen: string, evento: MatDatepickerInputEvent<Date>) {
    this._handleInput(evento.value);
  }

  onChangeTime(origen: string, evento: any) {
    this._handleInput(evento);
  }

  /** @internal */
  _handleInput(target: any): void {
    let value = target?.value !== undefined ? target?.value : target;
    if (!this._compositionMode || (this._compositionMode && !this._composing)) {
      value = this.writeValueFormat(value);
      if (this.onChange !== undefined)
        this.onChange(value);
      this.change.emit();
    }
  }

  /** @internal */
  _handlefocusOut(value: any): void {
    if (!this._compositionMode || (this._compositionMode && !this._composing) || this.type === "select") {
      value = this.writeValueFormat(value);
      this.writeValue(value);
      if (this.onChange !== undefined)
        this.onChange(value);
      this.change.emit();
    }
  }

  /** @internal */
  _compositionStart(): void { this._composing = true; }

  /** @internal */
  _compositionEnd(target: any): void {
    let value = target.value !== undefined ? target.value : target;
    this._composing = false;
    this._compositionMode = false;
    if (this.onChange !== undefined)
      this.onChange(value);
    this.change.emit();
  }

  /** @internal */
  _onEnter(): void {
    this.onEnter.emit(this);
  }

  /** @internal */
  _onFocus(): void {
    this.onFocus.emit(this);
  }

  registerOnChange(fn: (_: any) => void): void { this.onChange = fn; }

  registerOnTouched(fn: () => void): void { this.onTouched = fn; }

  setDisabledState?(isDisabled: boolean): void { this.disabled = isDisabled; }

  ngAfterViewInit(): void {
    this._composing = false;
    this._compositionMode = true;
    if (this.writeValueNoSet !== undefined) {
      this.writeValue(this.writeValueNoSet);
    }
  }
}
