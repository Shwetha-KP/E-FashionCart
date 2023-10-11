import { Component, OnInit, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  email: string = "";
  password: string = "";
  disablebutton = false;
  loginmessage = "";
  constructor(private router: Router,private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string) { }

  ngOnInit() {
  }
  login() {
    if (this.email == "" || this.password == "") {
      alert("Email or password cannot be empty");
    }
    else {
      this.disablebutton = true;
      this.loginmessage = "Please wait...";
      this.http.get<any>(this.baseUrl + `Customer/validateuser/${this.email}/${this.password}`).subscribe(result => {
        if (result != null) {
          if (result.status == -1) {
            alert("The Email " + this.email + " is is not registered!");
          }
          else if (result.status == 0) {
            alert("Password is incorrect!");
          }
          else if (result.status == 1) {
            localStorage.setItem("logintype", "customer");
            localStorage.setItem("Name", result.data.firstName);
            localStorage.setItem("Email", result.data.email);
            localStorage.setItem("CustomerId", result.data.id);
            this.router.navigateByUrl("/home");
          }
        }
        else {
          alert("Something Went wrong!");
        }
        this.loginmessage = "";
        this.disablebutton = false;
      }, error => { console.error(error); this.disablebutton = false; alert("Something Went wrong!"); this.loginmessage = "";});
    }
  }

}
