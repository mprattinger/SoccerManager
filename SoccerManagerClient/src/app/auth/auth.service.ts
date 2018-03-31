import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Router } from '@angular/router';
import { LoginModel } from './login-model';

@Injectable()
export class AuthService {

  private loggedIn: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);

  get isLoggedIn() {
    return this.loggedIn.asObservable();
  }

  constructor(private router: Router) { }

  login(user: LoginModel){
    this.loggedIn.next(true);
    this.router.navigate(['/']);
  }

  logout(){
    this.loggedIn.next(false);
  }
}
