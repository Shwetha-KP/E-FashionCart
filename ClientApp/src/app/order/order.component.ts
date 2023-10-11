import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit, Pipe, PipeTransform } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Agent } from 'https';

@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.css']
})
export class OrderComponent implements OnInit {
  order: Order;
  quantity = 0;
  agent: Agent;
  orderType: string;
  agentType: string;
  mobilenoerror: string = "";
  fullnameerror: string = "";
  pincodeerror: string = "";
  model: any;
  orderId: number;
  customerId: number;
  addressId: number;
  address: Address;
  addressList: Array<Address> = [];
  isCollapsed: boolean = true;
  disablebutton: boolean = false;

  
  public incrementQuantity() {
    this.quantity = this.quantity + 1;

  }
  public decrementQuantity() {
    if (this.quantity != 0) {
      this.quantity = this.quantity - 1;
    }
  }
  constructor(private http: HttpClient, @Inject('BASE_URL')
  private baseUrl: string, private router: Router, private route: ActivatedRoute) {
    this.customerId = +localStorage.getItem("CustomerId");
    this.orderId = route.snapshot.params.orderId == null ? 0 : + route.snapshot.params.orderId;
   /* this.getOrderItems();*/
    this.getAddresslist();
  }
  toggleCollapse() {
    this.isCollapsed = !this.isCollapsed;
    this.address = new Address();

    this.address.customerId = this.customerId;
    //this.address.addressId = this.addressId;
  }
  //getOrderItems() {
  //  this.http.get<any>(this.baseUrl + `Customer/getOrderItems/${this.customerId}`).subscribe(result => {

  //    console.log(result);
  //  }, error => console.error(error));

  //}
  validateFullname(fullName) {
    var namepattern = /^[A-Za-z .]{3,20}$/;
    if (namepattern.test(String(fullName).toString()) == true) {
      this.fullnameerror = "";
    }
    else {
      this.fullnameerror = "invalid name";
    }
  }
  validateMobileno(mobileNo) {
    var mblpattern = /^[789][0-9]{9}$/;
    if (mblpattern.test(String(mobileNo).toString()) == true) {
      this.mobilenoerror = "";
    }
    else {
      this.mobilenoerror = "invalid mobile number";
    }
  }
  validatePincode(pinCode) {
    var pincodepattern = /^[1-9]{1}[0-9]{2}\s{0,1}[0-9]{3}$/;
    if (pincodepattern.test(String(pinCode).toString()) == true) {
      this.pincodeerror = "";
    }
    else {
      this.pincodeerror = "invalid pincode";
    }
  }
  saveAddress() {
    if (this.address.fullName == "" || this.address.country == "" || this.address.state == "" || this.address.mobileNo == "" || this.address.pinCode == "" || this.address.address1 == "" || this.address.address2 == "" || this.address.landmark == "" || this.address.city == "") {
      alert("Please fill all the details..");
    }
    else if (this.mobilenoerror == "invalid mobile number" || this.fullnameerror == "invalid name" || this.pincodeerror =="invalid pincode") {
      alert("Please fill valid address details");
    }
    else {     
      this.address.id = 0;
      this.address.customerId = this.customerId;
      this.http.post<Address[]>(this.baseUrl + `Customer/postAddress`, this.address).subscribe(result => {
      this.isCollapsed = true;
      this.addressList = result;
      }, error => console.error(error));
    }
  }

  getAddresslist() {
    this.http.get<any>(this.baseUrl + `Customer/getAddresslist/${this.customerId}`).subscribe(result => {
      this.addressList = result;
    }, error => console.error(error));
  }
  cancelAddress() {
    this.isCollapsed = true;
  }
  ngOnInit() {
  }
  
  submit() {
    console.log(this.agentType);
    if (this.agentType ==null) {
      alert("Please Choose Preferred Agent for the measurement...");
    }

    else {
      let address = this.addressList.find(u => u.selected == true);
      console.log(address);
      if (address == null) {
        alert("Please Select Address");
      }
      else {
        let body = {
          agentType: this.agentType, orderId: this.orderId, addressId: address.id};
        console.log(body);
        this.http.post<any>(this.baseUrl + `Customer/confirmOrder`, body).subscribe(result => {
          localStorage.removeItem("orderId");
          alert("Ordered succefully");
          this.router.navigateByUrl("/home");
        }, error => console.error(error));
      }
    }
  }
  updateAddresss(row: Address) {
    let list = this.addressList.filter(u => u != row);
    if (list.length > 0) {
      for (let i = 0; i < list.length; i++) {
        list[i].selected = false;
      }
    }
    row.selected = true;
  }
}
class agent {
  agentcat: string;
  agentname: string;
  photo: string;
  mblno: string;
}
class Address {
  id: number;
  customerId: number;
  addressType: string;
  country: string;
  fullName: string;
  mobileNo: string;
  pinCode: string;
  address1: string;
  address2: string;
  landmark: string;
  city: string;
  state: string;
  selected: boolean;

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
