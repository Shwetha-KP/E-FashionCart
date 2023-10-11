import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { Local } from 'protractor/built/driverProviders';
import { DatePipe } from '@angular/common';
import { Router } from '@angular/router';
@Component({
  selector: 'app-agent-home',
  templateUrl: './agent-home.component.html',
  styleUrls: ['./agent-home.component.css']
})
export class AgentHomeComponent implements OnInit {
  agentusername:string;
  agentpassword: string;
  logintype: string;
  loggedin = false;
  data;
  order;
  address;
  selectedrow;
  displaytext = "Please click on Customer Name to view data.";
  items = [];
  measurements = [];
  detail = [];
  paymentModel: Payment;
  datePipe: DatePipe = new DatePipe("en-US");
  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string, private router: Router) {
    this.paymentModel = null;
    this.agentusername = localStorage.getItem("agentusername");
    this.agentpassword = localStorage.getItem("agentpassword");
    this.logintype = localStorage.getItem("logintype");
    if (this.logintype == "agent" && this.agentusername != null &&
      this.agentusername != "" && this.agentpassword != null &&
      this.agentpassword != "") {
      this.http.get<any>(this.baseUrl + `agent/agentlogin/${this.agentusername}/${this.agentpassword}`).subscribe(result => {
        if (result != null) {
          if (result.status == -1) {
            alert("Invalid agent!");
          }
          else if (result.status == 0) {
            alert("Wrong password!");
          }
          else if (result.status == 1) {
            this.loggedin = true;
            this.data = result.data;
            this.items = result.items;
          }
        }
        else {
          alert("Something went wrong!");
          this.loggedin = false;
        }
      }, error => { console.error(error); this.loggedin = false;});
    }
    else {
      this.loggedin = false;
    }
  }

  ngOnInit() {
  }
  login() {
    if (this.agentusername != null &&
      this.agentusername != "" && this.agentpassword != null &&
      this.agentpassword != "") {
      this.http.get<any>(this.baseUrl + `agent/agentlogin/${this.agentusername}/${this.agentpassword}`).subscribe(result => {
        if (result != null) {
          if (result.status == -1) {
            alert("Invalid agent!");
          }
          else if (result.status == 0) {
            alert("Wrong password!");
          }
          else if (result.status == 1) {
            this.loggedin = true;
            this.data = result.data;
            this.items = result.items;
            localStorage.setItem("agentusername", this.agentusername);
            localStorage.setItem("agentpassword", this.agentpassword);
            localStorage.setItem("logintype", this.logintype);
          }
        }
        else {
          alert("Something went wrong!");
          this.loggedin = false;
        }
      }, error => { console.error(error); this.loggedin = false; });
    }
    else {
      this.loggedin = false;
    }
  }
  getOrder(row) {
    this.displaytext = "Loading...";
    this.selectedrow = null;
    this.http.get<any>(this.baseUrl + `agent/getorder/${row.orderId}/${this.data.id}`).subscribe(result => {
      console.log(result);
      if (result != null) {
        if (result.status == -1) {
          this.displaytext = "";
          this.selectedrow = null;
          this.order = null;
          this.measurements = null;
          this.address = null;
          this.detail = null;
        }
        else if (result.status == 1) {
          this.displaytext = "";
          this.selectedrow = row;
          this.order = result.order;
          this.measurements = result.measure;
          this.address = result.address;
          this.detail = result.detail;
          if (this.order != null && this.order.orderStatus == 5) {
            this.paymentModel = new Payment();
            this.paymentModel.agentId = this.order.agentId;
            this.paymentModel.balance = 0;
            this.paymentModel.customerId = this.order.customerId;
            this.paymentModel.id = 0;
            //const orderdate = this.order.orderDate;
            this.paymentModel.orderDate = this.order.orderDate;
            //this.paymentModel.orderDate += this.datePipe.transform(orderdate, "hh:mm");
            //const dateCreated = this.paymentModel.orderDate.replace(' ', 'T');
            //const created = this.datePipe.transform(dateCreated, 'yyyy-MM-ddTHH:mm');
            this.paymentModel.orderId = this.order.id;
            this.paymentModel.paidAmount = 0;
            this.paymentModel.paymentMode = "";
            this.paymentModel.remarks = "";
            let totalamount = 0;
            for (let i = 0; i < this.detail.length; i++) {
              totalamount += (this.detail[i].price * this.detail[i].quantity);
              if (this.detail[i].stitchingAmount != null) {
                totalamount += this.detail[i].stitchingAmount;
              }
            }
            this.paymentModel.totalAmount = totalamount;
          }
          if (this.measurements.length > 0) {
            for (let i = 0; i < this.measurements.length; i++) {
              this.measurements[i].photo = this.detail.find(u => u.detailId==this.measurements[i].detailId).photo;
            }
          }          
        }
      }
      else {
        this.displaytext = "";
        this.selectedrow = null;
        this.measurements = null;
        this.order = null;
        this.address = null;
        this.detail = null;
      }
    }, error => {
      console.error(error); this.displaytext = "No Data!"; this.selectedrow = null;
      this.order = null;
      this.address = null;
      this.measurements = null;
      this.detail = null;});
  }
  addNew() {
    if (this.measurements == null) this.measurements = [];
    let measurement = { id: 0, orderId: this.order.id, partname: "", unitName: "cm", unitValue: 0, photo:"" };
    this.measurements.push(measurement);
  }
  saveMeasument(row) {
    console.log(row);
    if (typeof (row.detailId) == "string") row.detailId = +row.detailId;
    this.http.post<any>(this.baseUrl + `agent/addmeasure`, row).subscribe(result => {
      row.id = result.id;
      alert("Saved successfully!");
    }, error => { console.error(error); });
  }
  removeMeasument(row) {
    this.http.post<any>(this.baseUrl + `agent/removemeasure`, row).subscribe(result => {
      this.measurements = this.measurements.filter(u => u != u);
    }, error => { console.error(error); });
  }
  markAsCollected() {
    this.http.post<any>(this.baseUrl + `agent/PickedUp`, this.order).subscribe(result => {
      alert("Updated successfully!");
      this.displaytext = "";
      this.items = this.items.filter(u => u != this.selectedrow);
      this.selectedrow = null;
      this.measurements = null;
      this.order = null;
      this.address = null;
    }, error => { console.error(error); });
  }
  changePatternPhoto(row) {
    row.photo = this.detail.find(u=>u.detailId==row.detailId).photo;
  }
  updatePayment() {
    this.http.post<any>(this.baseUrl + `WeatherForecast/updatepayment`, this.paymentModel).subscribe(result => {
      if (result != null) {
        if (result.status == 1) {
          this.paymentModel = null;
          this.displaytext = "";
          this.items = this.items.filter(u => u != this.selectedrow);
          this.selectedrow = null;
          this.measurements = null;
          this.order = null;
          this.address = null;
          alert("Successfull!");
        }
        else if (result.status == 0) {
          alert("Something went wrong!");
        }
      }
      
     
    }, error => { console.error(error); });
  }
  logout() {
    localStorage.clear();
    this.router.navigateByUrl("/");
  }
}
export class Payment {
  public id: number;
  public orderId: number;
  public customerId: number;
  public agentId: number;
  public orderDate: string;
  public paymentDate: string;
  public totalAmount: number;
  public paymentMode: string;
  public paidAmount: number;
  public balance: number;
  public remarks: string;
}
