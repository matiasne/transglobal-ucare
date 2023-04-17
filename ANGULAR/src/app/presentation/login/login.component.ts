import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { lastValueFrom } from 'rxjs';
import { VERSION_APP } from '../../../version';
import { LoginApp } from '../../core/login/application/login.app';
import { Constants } from '../../shared/Constants';
import { ModelsService } from '../../shared/guard/model';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  public formLogin: FormGroup;
  public formRecuperar: FormGroup;
  public showError: boolean = false;
  public version = VERSION_APP;
  public versionServer = "";

  constructor(
    private formBuilder: FormBuilder,
    private loginApp: LoginApp,
    private router: Router,
    private modelsService: ModelsService,) {

    loginApp.getVersion().subscribe((r) => {
      this.versionServer = r;
    });

    this.formLogin = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', Validators.required],
    });

    this.formRecuperar = this.formBuilder.group({
      username: ['', Validators.required],
    });

  }

  ngOnInit(): void {
    this.modelsService.showHourglass = false;
  }

  public onClickIconPassword(event: any) {
    event.type = event.type === "password" ? "text" : "password";
    event.icon = event.type === "password" ? "visibility" : "visibility_off";
  }

  public showRecuperar = false;

  public onClickShowRecuperar() {
    this.showError = false;
    this.showRecuperar = true;
  }
  public onClickVolver() {
    this.showError = false;
    this.showRecuperar = false;
  }

  public async onClickRecuperar() {
    try {
      this.modelsService.user = null;
      this.showError = false;
      this.modelsService.showHourglass = true;
      if (this.formRecuperar.valid) {
        var resut = await lastValueFrom(this.loginApp.recuperar(this.formRecuperar.value.username));

        this.onClickVolver();
      }
    }
    catch (e) {
      this.showError = true;
    }
    this.modelsService.showHourglass = false;
  }

  public async onClickLogin() {
    try {
      this.modelsService.user = null;
      this.showError = false;
      this.modelsService.showHourglass = true;
      if (this.formLogin.valid) {
        await lastValueFrom(this.loginApp.login(this.formLogin.value.username, this.formLogin.value.password));
        
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
