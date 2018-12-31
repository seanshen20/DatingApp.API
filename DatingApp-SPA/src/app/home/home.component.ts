import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  register:boolean = false;
  constructor(private http: HttpClient) { }

  ngOnInit() {
    
  }

  toggle() {
    this.register = true;
    console.log("clicked");
  }

  cancelRegisterMode(registerMode: boolean) {
    this.register = registerMode;
  }

}
