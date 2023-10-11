import { DecimalPipe } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit, PipeTransform, Inject } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { CartComponent } from '../cart/cart.component';


@Component({
  selector: 'app-pattern',
  templateUrl: './pattern.component.html',
  styleUrls: ['./pattern.component.css']
})
export class PatternComponent implements OnInit {
  patternList: Array<Pattern> = [];
  id: number;
  orderId: number;
  productid: number = 0;
  disablebutton: boolean = false;
  order: Order;
  items: Array<DetailsOfOrder> = [];
  customerId: number;
  quantity = 0;
  customPattern: CustomPattern;
  constructor(private http: HttpClient, @Inject('BASE_URL')
  private baseUrl: string, private router: Router, private route: ActivatedRoute) {
    this.productid = +route.snapshot.params.productid;
    this.customerId = +localStorage.getItem("CustomerId");
    this.orderId = localStorage.getItem("orderId") == null ? 0 : +localStorage.getItem("orderId");
    if (this.orderId > 0) {
      this.getOrder();
    }
    this.getPattern();
    console.log(this.mypattern);
   
  }
  selectedrow: number;
  mypattern = [];
  ownPicture?: string = "";
  ngOnInit() {
  }
  getPattern() {
    this.http.get<Pattern[]>(this.baseUrl + `Customer/getPatterns/${this.productid}`).subscribe(result => {
      this.mypattern = result;
      console.log(result);

    }, error => console.error(error));
    
  }
  onSplideInit(splide) {
    console.log(splide);
  }
  plus() {
    this.quantity = this.quantity + 1;
  }
  minus() {
    if (this.quantity != 0) {
      this.quantity = this.quantity - 1;
    }

  }
  clickOrder(row?) {
    if (this.order == null) {
      this.order = new Order();
      this.order.agentId = 0;

    }
    if (this.items == null) {
      this.items = [];
    }
    this.order.customerId = this.customerId;
    this.order.id = this.order.id == null ? 0 : this.order.id;
    let patternType = "";
    if (this.customPattern != null && this.customPattern.photo != null && this.customPattern.photo != '') {
      patternType = "Custom";
    }
    else {
      patternType = "Default";
    }
    let detail = { id: 0, orderId: this.order.id, patternId: row == null ? 0 : row.id, quantity: row == null ? 0 : row.quantity, stitchingType: row == null ? "" : row.stitchingType, patternType: patternType };

    this.items.push(detail);
    let body = { order: this.order, details: this.items, customPattern: this.customPattern, patternType:patternType };
    this.http.post<any>(this.baseUrl + `Customer/addOrder`, body).subscribe(result => {
      this.order = result.order;
      localStorage.setItem("orderId", result.order.id);
      this.items = result.details;
      this.router.navigateByUrl("/order/" + this.order.id);

    }, error => console.error(error));
  }
  addtocart(row: Pattern) {
    if (this.order == null) {
      this.order = new Order();
      this.order.agentId = 0;     
    }
    if (this.items == null) {
      this.items =[];
    }
    this.order.customerId = this.customerId;   
    let detail = { id: 0, orderId: this.order.id, patternId: row.id, quantity: row.quantity, stitchingType: row.stitchingType, patternType: "Default" };
    this.items.push(detail);
    let body = { order: this.order, details: this.items };
    this.http.post<any>(this.baseUrl + `Customer/addCart`, body).subscribe(result => {
      this.order = result.order;
      localStorage.setItem("orderId", result.order.id);
      this.items = result.details;
      alert("Added To Cart");
    }, error => console.error(error));

  }
  getOrder() {
    this.http.get<any>(this.baseUrl + `Customer/getOrder/${this.orderId}`).subscribe(result => {
      this.order = result;
      
    }, error => console.error(error));
  }
 
 
  onFilechange(event) {
    if (event.target.files.length > 0) {
      let file = event.target.files[0];
      const reader = new FileReader();
      reader.onloadend = () => {
        const base64String = reader.result.toString()
          .replace("data:", "")
          .replace(/^.+,/, "");
        this.customPattern = new CustomPattern();
        this.customPattern.id = 0;
        this.customPattern.customerId = this.customerId;
        this.customPattern.productId = this.productid;
        this.customPattern.isActive = true;
        this.customPattern.quantity = 1;
        this.customPattern.price = 0;
        this.customPattern.photo = base64String;
      };
      reader.readAsDataURL(file);
    }
  }
  //ownpicture() {
  //  debugger;
  //  if (this.ownPicture == "") {
  //    alert("Please fill your own pattern!!");
  //  }
  //  else {
  //    this.disablebutton = true;
  //    let data = { ownPicture: this.ownPicture };
  //    this.http.post<any>(this.baseUrl + `Customer/OwnPicture`, data).subscribe(result => {
  //      alert("Uploaded Your Design");
  //      console.log(result);
  //    }, error => console.error(error));
  //      this.router.navigateByUrl("/order");

  //  }
  //}
}
class Pattern {
  id: number;
  productid: number;
  patternName: string;
  picture: string;
  picture1?: string;
  picture2?: string;
  picture3?: string;
  picture4?: string;
  price: number;
  stitchingType: string;
  quantity: number;
}
class Order {
  id: number;
  orderDate: string;
  customerId: number;
  orderStatus: number;
  agentId: number;
  agentType: string;

}
class DetailsOfOrder {
  id: number;
  orderId: number;
  patternId: number;
  quantity: number = 0;
  stitchingType: string;
  
}
class CustomPattern {
  id: number;
  customerId: number;
  productId: number;
  photo: string;
  description: string;
  isActive: boolean;
  quantity: number;
  stitchingType: string;
  price: number;
}
