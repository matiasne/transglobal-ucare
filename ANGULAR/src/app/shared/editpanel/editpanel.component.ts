import { Component, EventEmitter, Input, Output, OnInit, OnChanges } from "@angular/core";

@Component({
  selector: 'gq-editpanel',
  templateUrl: './editpanel.component.html',
  styleUrls: ['./editpanel.component.scss'],
})
export class EditPanelComponent implements OnInit, OnChanges {

  @Input() public windowsTitle = "Edición";
  @Input() public showEdit = false;
  @Input() public showReturn = true;
  @Input() public showCancel = true;
  @Input() public showSave = true;
  @Input() public showSaveAndContinue = false;
  @Input() public notShowEditAndSave = false;
  @Input() public showExport = false;
  @Input() public showHeader = true;
  @Input() public paddingfix: string = '1.25rem';


  @Output() onClickCancel: EventEmitter<object> = new EventEmitter();
  @Output() onClickSave: EventEmitter<object> = new EventEmitter();
  @Output() onClickSaveAndContinue: EventEmitter<object> = new EventEmitter();
  @Output() onClickExport: EventEmitter<object> = new EventEmitter();
  @Output() onClickEdit: EventEmitter<object> = new EventEmitter();
  @Output() onClickReturn: EventEmitter<object> = new EventEmitter();

  ngOnInit() {
    if (!this.notShowEditAndSave) {
      this.showEdit = !this.showSave
      this.showCancel = this.showSave;
    }
    else {
      this.showEdit = false;
    }
  }

  ngOnChanges() {
    this.showEdit = !this.showSave
    this.showCancel = this.showSave;
  }
  public onCancelButton(): void {
    this.onClickCancel.emit();
  }

  public onSave(): void {
    this.onClickSave.emit();
  }

  public onSaveAndContinue(): void {
    this.onClickSaveAndContinue.emit();
  }

  public onExport(): void {
    this.onClickExport.emit();
  }
  public onEdit(): void {
    this.showEdit = false;
    this.showCancel = true;
    this.showSave = true;
    this.showReturn = false;
    this.onClickEdit.emit();
  }
  public onReturn(): void {
    this.onClickReturn.emit();
  }
}
