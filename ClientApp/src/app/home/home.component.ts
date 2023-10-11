import { Component, Inject, OnInit, Pipe, PipeTransform  } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']

})
export class HomeComponent implements OnInit {

  productList: Array<Product> = [];
  patternList = [];
  selectedrow: number;
  id = 0;
  email: string;
  firstName: string;
  customer: any;

  constructor(private http: HttpClient, @Inject('BASE_URL')
  private baseUrl: string, private router: Router, private activatedroute: ActivatedRoute) {
    this.firstName = localStorage.getItem("Name");
    this.email = localStorage.getItem("Email");
    if (this.firstName == null || this.firstName == "" || this.email == null || this.email == "") {
      router.navigateByUrl("/");
    }
    this.http.get<any>(this.baseUrl + `Customer/getuser/${this.email}`).subscribe(result => {
      this.customer = result;
    }, error => console.error(error));
    this.getProduct();
  }

  ngOnInit() { }
  onSelect(row: Product) {
    if (row.select == true) {
      this.selectedrow = row.id;
    }
    else {
      this.selectedrow = null;
    }
    for (var i = 0; i <= this.productList.length; i++) {

      if (this.productList[i] == row) {
        this.productList[i].select = true;

      } else {
        this.productList[i].select = false;
      }
    }
  }
  onSplideInit(splide) {
    console.log(splide);
  }
  getProduct() {
    this.http.get<Product[]>(this.baseUrl + `Customer/getProducts`).subscribe(result => {
      this.productList = result;
      console.log(result);
    }, error => console.error(error));
  }
  gotoPattern(row) {
    this.router.navigateByUrl("/patterns/"+row.id);
  }
}

class Product {
     
      id: number;
      productName: string;
      category: string;
      picture?: string;
      price: number;
    select?: boolean;
}

@Pipe({
  name: 'filterprod',
  pure: false
})
export class FilterProd implements PipeTransform {
  transform(items: Array<any>, productid: number, price: number) {
    if (productid == null)
      return [];
    else {
      return items.filter(u => u.productid == productid);
    } 
  }
  }

