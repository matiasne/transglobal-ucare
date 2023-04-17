import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'dialog-alert',
  templateUrl: './alert.component.html',
  styleUrls: ['./alert.component.scss'],
})
export class AlertComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<AlertComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) { }

  onAceptar(): void {
    this.dialogRef.close();
  }

  ngOnInit() {
    if (this.data.buttonDeleteLabel == undefined)
      this.data.buttonDeleteLabel = "Cancelar";
    if (this.data.buttonAceptLabel == undefined)
      this.data.buttonAceptLabel = "Aceptar";
  }

  public getIcon() {
    switch (this.data.type) {
      case "warning": return "assets/images/icons/warning-icon.svg";
      case "info": return "assets/images/icons/info-icon.svg";
      case "error": return "assets/images/icons/error-icon.svg";
      case "confirmation": return "assets/images/icons/confirmation-icon.svg";
      case "question": return "assets/images/icons/question-icon.svg";
    }
    return "";
  }
  public getColor() {
    switch (this.data.type) {
      case "warning": return "#EEA236";
      case "info": return "#0C78CC";
      case "error": return "#ff5050";
      case "confirmation": return "#4CAf50";
      case "question": return "#87adbd";
    }
    return "";
  }
  public getOrder() {
    switch (this.data.type) {
      case "warning": return "0";
      case "info": return "0";
      case "error": return "0";
      case "confirmation": return "0";
      case "question": return "2";
    }
    return "";
  }

}
