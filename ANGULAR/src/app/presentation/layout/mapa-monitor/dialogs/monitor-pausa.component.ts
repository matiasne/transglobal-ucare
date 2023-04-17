import { Component, Inject, OnDestroy } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import * as moment from 'moment';
import { Monitor } from '../mapa-monitor.component';

@Component({
  selector: 'dialog-monitor-pausa',
  templateUrl: './monitor-pausa.component.html',
  styleUrls: ['./monitor-pausa.component.scss'],
})
export class MonitorPausaComponent implements OnDestroy {
  public alertInterval: any;
  public countCownText: string = "";

  constructor(
    public dialogRef: MatDialogRef<MonitorPausaComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { date: Date, data: Monitor }) {

    this.countDown(data.date);
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
    let duration = moment.duration((new Date()).getTime() - fechaMax.toDate().getTime() + (fechaMax.toDate().getTimezoneOffset()*60000) , 'milliseconds');

    this.alertInterval = setInterval(() => {
      try {

        duration = duration.add(1000, 'milliseconds');
        this.countCownText = duration.hours() + ":" + duration.minutes() + ":" + duration.seconds();
      }
      catch {

      }

    }, 1000);
  }
}
