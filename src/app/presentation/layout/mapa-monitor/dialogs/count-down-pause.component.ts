import { Component, Inject, OnDestroy } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import * as moment from 'moment';

@Component({
  selector: 'dialog-count-down-pause',
  templateUrl: './count-down-pause.component.html',
  styleUrls: ['./count-down-pause.component.scss'],
})
export class CountDownPauseAlertaComponent implements OnDestroy {
  public alertInterval: any;
  public countCownText: string = "";

  constructor(
    public dialogRef: MatDialogRef<CountDownPauseAlertaComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Date) {

    this.countDown(data);
  }

  ngOnDestroy(): void {
    clearInterval(this.alertInterval);
  }

  public onCancel(): void {
    this.dialogRef.close(null);
  }
  public onAceptar(): void {
    this.dialogRef.close(true);
  }

  public countDown(date: Date) {

    let fechaMax = moment(date);
    let duration = moment.duration(fechaMax.toDate().getTime() - (new Date()).getTime(), 'milliseconds');

    this.alertInterval = setInterval(() => {
      try {

        duration = duration.subtract(1000, 'milliseconds');
        this.countCownText = duration.hours() + ":" + duration.minutes() + ":" + duration.seconds();

        if (duration.asSeconds() < 2) {
          this.dialogRef.close(false);
        }
      }
      catch {

      }

    }, 1000);
  }
}
