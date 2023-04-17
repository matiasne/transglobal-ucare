import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { lastValueFrom } from 'rxjs';
import { VERSION_APP } from '../../../version';
import { LoginApp } from '../../core/login/application/login.app';
import { Constants } from '../../shared/Constants';
import { ModelsService } from '../../shared/guard/model';

@Component({
  selector: 'app-recuperar',
  templateUrl: './recuperar.component.html',
  styleUrls: ['./recuperar.component.scss']
})
export class RecuperarComponent implements OnInit {

  public formLogin: FormGroup;
  public showError: boolean = false;
  public version = VERSION_APP;
  public versionServer = "";
  public id: string = "";

  constructor(
    private formBuilder: FormBuilder,
    private loginApp: LoginApp,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private modelsService: ModelsService,) {

    loginApp.getVersion().subscribe((r) => {
      this.versionServer = r;
    });

    this.formLogin = this.formBuilder.group({
      codigo: ['', Validators.required],
      password1: ['', Validators.required],
      password2: ['', Validators.required],
    });
  }

  ngOnInit(): void {
    this.modelsService.showHourglass = false;
    this.activatedRoute.queryParams.subscribe((param: any) => {
      this.id = param.id
    })
  }

  public onClickIconPassword(event: any) {
    event.type = event.type === "password" ? "text" : "password";
    event.icon = event.type === "password" ? "visibility" : "visibility_off";
  }

  public async onClickCambiar() {
    try {
      this.modelsService.user = null;
      this.showError = false;
      this.modelsService.showHourglass = true;
      if (this.formLogin.valid) {
        await lastValueFrom(this.loginApp.recuperarCambiar(this.id, this.formLogin.value.codigo, this.formLogin.value.password1));
        
        this.router.navigate([Constants.LAYOUT_HOME]);
      }
      else {
        throw new Error("No valid data");
      }

    } catch (e) {
      this.showError = true;
    }

    this.modelsService.showHourglass = false;
  }

}
