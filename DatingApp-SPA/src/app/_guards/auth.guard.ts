import { Injectable } from '@angular/core';
import { CanActivate, Router} from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router, private altertify: AlertifyService) {  }
  canActivate(): boolean {
    if (this.authService.loggedIn()) {
      return true;
    }
    this.altertify.error('You shall not pass !!!');
    this.router.navigate(['/home']);
    return false;
  }

}
