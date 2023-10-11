import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { Router } from '@angular/router';
@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {
  email: string;
  password: string;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string, private router: Router) { }
  ngOnInit() {
  }
  submit() {
    debugger;
    if (this.email == "" || this.password == "") {
      alert("Email or password cannot be empty");
    }
    else {
      this.http.get<any>(this.baseUrl + `weatherforecast/Adminlogin/${this.email}/${this.password}`).subscribe(result => {
        if (result != null) {
          localStorage.setItem("logintype", "admin");
          localStorage.setItem("Name", result.Name);
          localStorage.setItem("Email", result.Email);
          this.router.navigateByUrl("/adminproduct");
        }
        else {
          alert(" Admin login failed.Please check Email and Password!");
        }
      }, error => console.error(error));
    }
  }
}

