import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Local } from 'protractor/built/driverProviders';


@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css']
})
export class CartComponent implements OnInit {
  cartItem: Array<any> = [];
  cartTotal = 0;
  productid: number;
  quantity: number = 0;
  order: Order;
  items: Array<DetailsOfOrder> = [];
  cartItems = [];
  customerId: number;
  constructor(private http: HttpClient, @Inject('BASE_URL')
  private baseUrl: string, private router: Router, private route: ActivatedRoute) {
    this.customerId = +localStorage.getItem("CustomerId");
    this.getCartItems();
    /*  console.log(this.cartItems);*/
  }
  ngOnInit() {
  }
  onSplideInit(splide) {
    console.log(splide);
  }

  getCartItems() {
    this.http.get<any>(this.baseUrl + `Customer/getCartItems/${this.customerId}`).subscribe(result => {
      this.cartItem = result;
      console.log(result);
    }, error => console.error(error));

  }
  //removeItem(detail) {
  //  for (let i = 0; i < this.cartItems.length; ++i) {
  //    if (this.cartItem[i].id === this.id) {
  //      this.cartItem.splice(i, 1);
  //    }
  //  }
  //}

  removeItem(cart, detail) {
    if (detail.id == null || detail.id == 0) {
      cart.orderDetail = cart.orderDetail.filter(prod => prod != detail);
    }
    else {

      this.http.post<any>(this.baseUrl + `Customer/deleteItem`, detail).subscribe(result => {
        cart.orderDetail = cart.orderDetail.filter(prod => prod != detail);
        alert("Removed Successfully");
      }, error => console.error(error));
    }
  }
  deleteAll(detail) {
    this.http.post<any>(this.baseUrl + `Customer/deleteOrder`, detail).subscribe(result => {
      this.cartItem = this.cartItem.filter(prod => prod != detail);
      alert("Deleted Successfully!!");
    }, error => console.error(error));
  }
  gotoOrder(cart) {
    this.router.navigateByUrl("/order/" + cart.id);
  }
  plus() {
    this.quantity = this.quantity + 1;
  }
  minus() {
    if (this.quantity != 0) {
      this.quantity = this.quantity - 1;
    }
  }
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
  quantity: number;
}
class Order {
  id: number;
  orderDate: string;
  customerId: number;
  orderStatus: number;
}
class DetailsOfOrder {
  id: number;
  orderId: number;
  patternId: number;
  
  quantity: number;
  stitchingType: string;
  agentId: number;
}
