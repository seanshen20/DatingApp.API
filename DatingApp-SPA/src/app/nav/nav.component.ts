import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  username: any;
  model: any = {};
  constructor(private authService: AuthService, private alertifyService: AlertifyService) {
  }
  
  ngOnInit() {
   
  }

  login() {
    this.authService.login(this.model)
      .subscribe(
      next => {
        this.alertifyService.success('Logged in sucessfully');
      },
      error => this.alertifyService.error(error))
  }

  loggedIn() {
    this.username = this.authService.getName();
    return this.authService.loggedIn();
  }

  loggedOut() {
    localStorage.removeItem('token');
    this.alertifyService.message('logged out');
}
}
