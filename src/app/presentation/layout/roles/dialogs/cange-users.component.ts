import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { lastValueFrom } from 'rxjs';
import { RolesApp } from '../../../../core/roles/application/roles.app';
import { RolesEntity } from '../../../../core/roles/domain/roles.entity';

@Component({
  selector: 'dialog-cange-users',
  templateUrl: './cange-users.component.html',
  styleUrls: ['./cange-users.component.scss'],
})
export class ChageUsersComponent implements OnInit {

  public datagrid: Array<RolesEntity> = [];

  constructor(
    public dialogRef: MatDialogRef<ChageUsersComponent>,
    public formBuilder: FormBuilder,
    public appCrud: RolesApp,
    @Inject(MAT_DIALOG_DATA) public data: { icon: string, labeInput: string, header1: string, header2: string, buttonAction: string, entity: RolesEntity, onAction: Function }) {
  }

  public showLoadingOverlay = true;
  public selected: any = null;

  ngOnInit(): void {
    this.onLoadData();
  }

  public async onLoadData() {
    try {
      var resul = (await lastValueFrom(this.appCrud.GetReplace(this.data.entity.id ?? "")));
      if (!resul.isError)
        this.datagrid = resul.data ?? [];
      else {

      }
    }
    catch (error) {

    }
    this.showLoadingOverlay = false;
  }

  public onClickAction() {
    this.dialogRef.close(null);
    this.data.onAction(this.selected);
  }

  public onCancel(): void {
    this.dialogRef.close(null);
  }

}
