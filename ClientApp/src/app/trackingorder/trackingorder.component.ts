import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-trackingorder',
  templateUrl: './trackingorder.component.html',
  styleUrls: ['./trackingorder.component.css']
})
export class TrackingorderComponent implements OnInit {
  tracklist:Array<any>= [];
  order: Order;
  customerId: any;
  items: Array<DetailsOfOrder> = [];
  completedOrdersList = [];
  currentOrdersList = [];
  index: number=1;
  constructor(private http: HttpClient, @Inject('BASE_URL')
  private baseUrl: string, private router: Router, private route: ActivatedRoute) {
    this.customerId = localStorage.getItem("CustomerId");
    if (this.customerId == null || this.customerId == undefined || this.customerId=="") {
      this.router.navigateByUrl("/");
    }
    else {
      this.customerId = +this.customerId;
      this.index = 1;
      this.getCustomerCurrentOrders();
    }
   // this.getTrackList();
  }

  ngOnInit() {
  }

  getTrackList() {
    this.http.get<any>(this.baseUrl + `Customer/trackOrder/${this.customerId}`).subscribe(result => {
      console.log(result);
      this.tracklist = result;
    }, error => console.error(error));
  }
  getCustomerCurrentOrders() {
    this.http.get<any>(this.baseUrl + `customer/getcustomercurrentorders/${this.customerId}`).subscribe(result => {
      this.currentOrdersList = result;
      console.log(result);
    }, error => { console.error(error); });
  }
  getCustomerOrderHistories() {
    this.http.get<any>(this.baseUrl + `customer/getcustomercompletedorders/${this.customerId}`).subscribe(result => {
      this.completedOrdersList = result;
    }, error => { console.error(error); });
  }
  changeIndex(index: number) {
    this.index = index;
    switch (this.index) {
      case 1:
        this.getCustomerCurrentOrders();
        break;
      case 2:
        this.getCustomerOrderHistories()
        break;
    }
  }
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
class agent {
  id: number;
  deliveryAgent: string;
  pickupAgent: string;
  adharNumber: string;
  mobileNo: string;
  email: string;
  photo: string;
  address: string;
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
