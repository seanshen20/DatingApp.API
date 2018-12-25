import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ValueService } from './value.service';

@Component({
  selector: 'app-values',
  templateUrl: './values.component.html',
  styleUrls: ['./values.component.css']
})
export class ValuesComponent implements OnInit {
  values: any;
  persons: any;

  constructor(private valueService: ValueService) {  };

  ngOnInit() {
    this.getValue();
  };

  getValue() {
    this.valueService.getValues()
      .subscribe(v => this.values = v);
    
  }

}
