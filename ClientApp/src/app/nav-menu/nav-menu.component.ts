import { HttpClient } from '@angular/common/http';
import { Component, Inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {

  count: any;
  constructor(private http: HttpClient, @Inject('BASE_URL')
  private baseUrl: string, private router: Router, private route: ActivatedRoute) {
    this.getCountList();
  }
  getCountList() {
    this.http.get<any>(this.baseUrl + `Customer/getCountList`).subscribe(result => {
      this.count = result.count;
      console.log(result.count);
    }, error => console.error(error));
  }
  isExpanded = false;

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}

