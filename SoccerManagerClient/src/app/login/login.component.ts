import { Component, OnInit } from '@angular/core';
import { LoginModel } from '../auth/login-model';
import { AuthService } from '../auth/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  model = new LoginModel();

  constructor(private authService: AuthService) { }

  ngOnInit() {
  }

  onSubmit(){
    this.authService.login(this.model);
  }
}
