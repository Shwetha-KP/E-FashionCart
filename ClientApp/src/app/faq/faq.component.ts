import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-faq',
  templateUrl: './faq.component.html',
  styleUrls: ['./faq.component.css']
})
export class FAQComponent implements OnInit {

  constructor() { }
  
  ngOnInit() {
  }
  select: any = '';
  accordion(ids: any) {
    
    if (this.select == ids) {
      this.select = '';
    }
    else {
      this.select = ids;
    }
  }
}
