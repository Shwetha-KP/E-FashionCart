import { Component, Inject, OnInit, Pipe, PipeTransform } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';


@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css']
})
export class ProductComponent implements OnInit {
  productList= [];
  patternlist = [{ id: 1, productid: 1, name: 'round neck', picture: "roundneck.jpg" },
  { id: 2, productid: 1, name: 'square neck', picture: "111690-200.png" },
  { id: 3, productid: 2, name: 'round neck', picture: "roundneck.jpg" },
  { id: 4, productid: 2, name: 'sleevless', picture: "sleeveless.jpg" }];
  selectedrow: number;
  id = 0;
  constructor(private http: HttpClient, @Inject('BASE_URL')
  private baseUrl: string, private router: Router, private activatedroute: ActivatedRoute) { }

  ngOnInit() {
  }
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
  getProduct() {
    this.http.get<Product[]>(this.baseUrl + `Customer/getProducts`).subscribe(result => {
      this.productList = result;
      console.log(result);
    }, error => console.error(error));
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
//@Pipe({
//  name: 'filterprod',
//  pure: false
//})
//export class FilterProd implements PipeTransform {
//  transform(items: Array<any>, productid: number) {
//    if (productid == null) return [];
//    else {
//      return items.filter(u => u.productid == productid);
//    }
//  }
//}
