import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit, Pipe, PipeTransform } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { OrderComponent } from '../order/order.component';

@Component({
  selector: 'app-product',
  templateUrl: './adminproduct.component.html',
  styleUrls: ['./adminproduct.component.css']
})
export class AdminproductComponent implements OnInit {
  productList: Array<Product> = [];
  newProduct: Product;
  picture: string;
  patternList: Array<Pattern> = [];
  newPattern: Pattern;
  Picture: string;
  agentList: Array<Agent> = [];
  newAgent: Agent;
  Photo: string;
  addressId: number;
  orderDetails=[];
  workInProgress=[];
  orderlist: Array<Order> = [];
  workinOrderList = [];
  selectedOrder: Order;
  workinDetailList = [];
  selectedDetail = null;
  workinMeasureList = [];
  pendingPickupList = [];
  pendingDeliveryList = [];
  assignDelivery = [];
  completedOrdersList = [];
  displaytext = "No data selected!";
  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string, private router: Router, private route: ActivatedRoute) {
  }
 ngOnInit() {
    this.newProduct = new Product();
    this.newPattern = new Pattern();
   this.newAgent = new Agent();

  }
  saveProduct(row: Product) {
    debugger;
    if (row.productName == "" || row.picture == "") {
      alert("Please enter the data");
    }

    else {

      this.http.post<any>(this.baseUrl + `weatherforecast/product`, row).subscribe(result => {
        row = result;
      }, error => console.error(error));
    }
  }
  savePattern(row: Pattern) {
    debugger;
    if (row.productId == null || row.patternName == "" || row.picture == "" ||
      row.price == null) {
      alert("please enter pattern");
    }

    else {
      row.productId = +row.productId;
      console.log(row);
      this.http.post<any>(this.baseUrl + `weatherforecast/pattern`, row).subscribe(result => {
        row = result;
      }, error => console.error(error));
    }
  }
  saveAgent(row: Agent) {
    debugger;
    if (row.deliveryAgent == null || row.adharNumber == null || row.photo == null ||
      row.email == null || row.mobileNumber == null || row.address == null || row.password == null ||
      row.deliveryAgent == "" || row.adharNumber == "" || row.photo == "" ||
      row.email == "" || row.mobileNumber == "" || row.address == "" || row.password == "") {
      alert("Please fill agent information");
    }
    else {
      this.http.post<any>(this.baseUrl + `weatherforecast/Agent`, row).subscribe(result => {
        row = result;
        alert("Saved Successfull!");
      }, error => console.error(error));
    }
  }
 addProduct(row) {
    this.productList.push(new Product());
  }  
deleteProduct(row : Product) {

  
  this.http.post<any>(this.baseUrl + `weatherforecast/deleteproduct`, row).subscribe(result => {
    this.productList = this.productList.filter(prod => prod != row);
  }, error => console.error(error));
   
  }
  editProduct(row:Product) {
    row.edit = !row.edit;
  }
  addPattern(row) {
    this.patternList.push(new Pattern());
  }
  deletePattern(row: Pattern) {

    if (row.id == null || row.id == 0) {
      this.patternList = this.patternList.filter(prod => prod != row);
    }
    else {
      this.http.post<any>(this.baseUrl + `weatherforecast/deletepattern`, row).subscribe(result => {
        this.patternList = this.patternList.filter(prod => prod != row);
      }, error => console.error(error));


    }

  }
  editPattern(row: Pattern) {
    row.edit = !row.edit;
  }
  addAgent(row) {
    this.agentList.push(new Agent());
  }
  deleteAgent(row:Agent) {

    if (row.id == null || row.id == 0) {
      this.agentList = this.agentList.filter(prod => prod != row);
    }
    else {
      this.http.post<any>(this.baseUrl + `weatherforecast/deleteagent`, row).subscribe(result => {
        this.agentList = this.agentList.filter(prod => prod != row);
      }, error => console.error(error));

    
    }

  }
  editAgent(row: Agent) {
    row.edit = !row.edit;
  }
  getProduct() {
    this.http.get<Product[]>(this.baseUrl + `weatherforecast/getProducts`).subscribe(result => {
      this.productList = result;
      console.log(result);
    }, error => console.error(error));
  }
  getPattern() {
    this.http.get<Pattern[]>(this.baseUrl + `weatherforecast/getPatterns`).subscribe(result => {
      this.patternList= result;
      console.log(result);
    }, error => console.error(error));
  }
  getAgent() {
    this.http.get<Agent[]>(this.baseUrl + `weatherforecast/getAgents`).subscribe(result => {
      console.log(result);
      this.agentList = result;
      console.log(result);
    }, error => console.error(error));
  }
  getAvailableAgent() {
    this.http.get<Agent[]>(this.baseUrl + `weatherforecast/GetAvailableAgents`).subscribe(result => {
      console.log(result);
      this.agentList = result;
      console.log(result);
    }, error => console.error(error));
  }
   uploadphoto(event,row) {
    if (event.target.files.length > 0) {
      let file = event.target.files[0];
      const reader = new FileReader();
      reader.onloadend = () => {
        const base64String = reader.result.toString()
          .replace("data:", "")
          .replace(/^.+,/, "");
      row.picture = base64String;
      };
      reader.readAsDataURL(file);
    }
  }
  uploadimage(event, row) {
    if (event.target.files.length > 0) {
      let file = event.target.files[0];
      const reader = new FileReader();
      reader.onloadend = () => {
        const base64String = reader.result.toString()
          .replace("data:", "")
          .replace(/^.+,/, "");
        row.picture = base64String;
      };
      reader.readAsDataURL(file);
    }
  }
  uploadimage1(event, row) {
    if (event.target.files.length > 0) {
      let file = event.target.files[0];
      const reader = new FileReader();
      reader.onloadend = () => {
        const base64String = reader.result.toString()
          .replace("data:", "")
          .replace(/^.+,/, "");
        row.picture1 = base64String;
      };
      reader.readAsDataURL(file);
    }
  }
  uploadimage2(event, row) {
    if (event.target.files.length > 0) {
      let file = event.target.files[0];
      const reader = new FileReader();
      reader.onloadend = () => {
        const base64String = reader.result.toString()
          .replace("data:", "")
          .replace(/^.+,/, "");
        row.picture2= base64String;
      };
      reader.readAsDataURL(file);
    }
  }
  uploadimage3(event, row) {
    if (event.target.files.length > 0) {
      let file = event.target.files[0];
      const reader = new FileReader();
      reader.onloadend = () => {
        const base64String = reader.result.toString()
          .replace("data:", "")
          .replace(/^.+,/, "");
        row.picture3 = base64String;
      };
      reader.readAsDataURL(file);
    }
  }
  uploadimage4(event, row) {
    if (event.target.files.length > 0) {
      let file = event.target.files[0];
      const reader = new FileReader();
      reader.onloadend = () => {
        const base64String = reader.result.toString()
          .replace("data:", "")
          .replace(/^.+,/, "");
        row.picture4= base64String;
      };
      reader.readAsDataURL(file);
    }
  }
  uploadpicture(event,row) {
    if (event.target.files.length > 0) {
      let file = event.target.files[0];
      const reader = new FileReader();
      reader.onloadend = () => {
        const base64String = reader.result.toString()
          .replace("data:", "")
          .replace(/^.+,/, "");
       row.photo= base64String;
      };
      reader.readAsDataURL(file);
    }
  }
  openCity(evt,cityName) {
    var i, tabcontent, tablinks;
    tabcontent = document.getElementsByClassName("tabcontent");
    for (i = 0; i < tabcontent.length; i++) {
      tabcontent[i].style.display = "none";
    }
    tablinks = document.getElementsByClassName("tablinks");
    for (i = 0; i < tablinks.length; i++) {
      tablinks[i].className = tablinks[i].className.replace(" active", "");
    }
    document.getElementById(cityName).style.display = "block";
    if (evt!=null && evt.currentTarget != null) {
      evt.currentTarget.className += " active";
    }
    switch (cityName) {
      case "Product": this.getProduct();break;
      case "Pattern": this.getPattern(); break;
      case "Agent": this.getAgent(); break;
      case "AssignPickup": this.getAvailableAgent(); this.getDetailsOfOrder();break;
      case "PendingPickup": this.pendingpickup();break;
      case "WorkInProgress": this.workinprogressorder();  break;
      case "AssignDelivery": this.getAvailableAgent(); this.assignDeliveries(); break;
      case "PendingDelivery": this.pendingdelivery();break;
      case "Completed": this.getAllCompletedOrders(); break;

    }
  }
  getDetailsOfOrder() {
    this.http.get<any>(this.baseUrl + `weatherforecast/getorderDetails`).subscribe(result => {
      this.orderDetails = result;
      console.log(result);

    }, error => console.error(error));

  }
  assignDeliveries() {
    this.http.get<any>(this.baseUrl + `weatherforecast/assigndelivery`).subscribe(result => {
      this.assignDelivery = result;
      console.log(result);

    }, error => console.error(error));

  }
  getstitchingworks() {
    this.http.get<any>(this.baseUrl + `weatherforecast/getstitchingworks`).subscribe(result => {
      this.workInProgress= result;
      console.log(result);

    }, error => console.error(error));

  }
  
  onSplideInit(splide) {
    console.log(splide);
  }
  updatePendingOrderStatus(details: Order, pending: DetailsOfOrder) {
    this.http.post<any>(this.baseUrl + `weatherforecast/savependingorderstatus/${pending.id}`, details).subscribe(result => {
      this.orderDetails = this.orderDetails.filter(u => u != details);
    }, error => console.error(error));
  }
  updatePendingDeliveryOrderStatus(details: Order, pending: DetailsOfOrder) {
    this.http.post<any>(this.baseUrl + `weatherforecast/savependingdeliveryorderstatus/${pending.id}`, details).subscribe(result => {
      this.assignDelivery = this.assignDelivery.filter(u => u != details);
    }, error => console.error(error));
  }
  
  clearAll() {
    this.selectedOrder = null;
    this.selectedDetail = null;
    this.workinDetailList = [];
    this.workinMeasureList = [];
  }
  pendingpickup() {
    this.http.get<any>(this.baseUrl + `weatherforecast/pendingpickup/`).subscribe(result => {
      this.pendingPickupList = result;
      console.log(result);
    }, error => { console.error(error); });
  }
  pendingdelivery() {
    this.http.get<any>(this.baseUrl + `weatherforecast/pendingdeliver/`).subscribe(result => {
      this.pendingDeliveryList = result;
      console.log(result);
    }, error => { console.error(error); });
  }
  
  workinprogressorder() {
    this.http.get<any>(this.baseUrl + `weatherforecast/workinprogressorder/`).subscribe(result => {
      this.workinOrderList = result;
      this.selectedOrder = null;
      this.selectedDetail = null;
      this.workinMeasureList=[]
    }, error => { console.error(error); });
  }
  getWorkinDetail(row: Order) {
    this.selectedOrder = row;
    this.workinMeasureList = [];
    this.workinprogressdetail();
  }
  getWorkinMeasure(row: DetailsOfOrder) {
    this.selectedDetail = row;
    this.workinprogressmeasure();
  }
  workinprogressdetail() {
    this.http.get<any>(this.baseUrl + `weatherforecast/workinprogressdetail/${this.selectedOrder.id}`).subscribe(result => {
      this.workinDetailList = result;
    }, error => { console.error(error); });
  }
  markAsStitchingCompleted(row) {
    this.http.post<any>(this.baseUrl + `weatherforecast/SaveWorkinDetailData`, row.det).subscribe(result => {
      alert("Saved Successfully!");
      this.workinDetailList = this.workinDetailList.filter(u => u != row);
      this.workinMeasureList = [];
    }, error => { console.error(error); });
  }
  workinprogressmeasure() {
    this.http.get<any>(this.baseUrl + `weatherforecast/workinprogressmeasure/${this.selectedOrder.id}/${this.selectedDetail.det.id}`).subscribe(result => {
      this.workinMeasureList = result;
    }, error => { console.error(error); });
  }
  getAllCompletedOrders() {
    this.http.get<any>(this.baseUrl + `weatherforecast/getallcompletedorders`).subscribe(result => {
      this.completedOrdersList = result;
    }, error => { console.error(error); });
  }
  logout() {
    localStorage.clear();
    this.router.navigateByUrl("/admin");
  }

}

class Product {
  id: number;
  productName: string;
  picture: string;
  edit: boolean;
}
class Pattern {
  id: number;
  productId:number;
  patternName: string;
  picture: string;
  picture1?: string;
  picture2?: string;
  picture3?: string;
  picture4?: string;
  price: number;
  edit: boolean;
}
class Agent {
  id: number;
  deliveryAgent: string;
  adharNumber: string;
  gender: string;
  password: string;
  photo: string;
  email: string;
  mobileNumber: string;
  address: string;
  edit: boolean;
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
  quantity: number = 0;
  stitchingType: string;
  agentId: number;
  status: boolean;
  stitchingAmount: number;
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

}

@Pipe({
  name: 'filterggender',
  pure: false
})
export class FilterGender implements PipeTransform {
  transform(items: Array<Agent>, gender:string) {
    if (gender == null||gender=="")
      return items;
    else {
      return items.filter(u => u.gender == gender);
    }
  }
}






