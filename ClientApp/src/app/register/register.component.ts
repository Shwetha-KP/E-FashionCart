import { Component, OnInit, Inject } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  firstName: string = "";
  nameerror: string = "";
  lastName: string = "";
  email: string = "";
  emailerror: string = "";
  mobileno: string = "";
  mblnoerror: string = "";
  password: string = "";
  passworderror: string = "";
  confirmpassword: string = "";
  photo: string = "";
  disablebutton: boolean = false;


  constructor(private router: Router, private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string) { }

  ngOnInit() {
  }
  validateEmail(email) {
    var re = /\S+@\S+\.\S+/;
    if (re.test(String(email).toLowerCase()) == true) {
      this.emailerror = "";
    }
    else {
      this.emailerror = "invalid email";
    }   
  }
  validateMobileno(mobileno) {
    var mblpattern = /^[789][0-9]{9}$/;
    if (mblpattern.test(String(mobileno).toString()) == true) {
      this.mblnoerror = "";
    }
    else {
      this.mblnoerror = "invalid mobile number";
    } 
  }
  validateName(firstName) {
    var namepattern = /^[A-Za-z .]{3,20}$/;
    if (namepattern.test(String(firstName).toString()) == true) {
      this.nameerror = "";
    }
    else {
      this.nameerror = "invalid firstname";
    }
  }
  validatePassword(password) {
    var passwordpattern = /^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$/;
    if (passwordpattern.test(String(password).toString()) == true) {
      this.passworderror = "";
    }
    else {
      this.passworderror = "invalid password";
    }
  }
  
  register() {
    debugger;
    if (this.password != this.confirmpassword) {
      alert("Password is not matching!");
    }
    else if (this.firstName == "" || this.lastName == "" || this.password == "" || this.email == "" || this.mobileno == "") {
      alert("Please fill all the informations!");
    }
    else if (this.emailerror == "invalid email" || this.mblnoerror == "invalid mobile number" || this.nameerror == "invalid firstname" || this.passworderror == "invalid password") {
      alert("Please fill valid Firstname, Email, mobile number or password");
    }

    else {
      this.disablebutton = true;
      let body = {
        firstName: this.firstName, lastName: this.lastName, email: this.email,
        mobileno: this.mobileno, password: this.password, photo: this.photo
      }
      this.http.post<any>(this.baseUrl + `Customer/RegisterCustomer`, body).subscribe(result => {
        if (result != null) {
          if (result.status == -1) {
            alert("Customer Email " + this.email + " is already registered!");
          }
          else if (result.status == 0) {
            alert("Unable to register due to network issue. Please try again after sometime!");
          }
          else if (result.status == 1) {
            alert("Your account created successfully!");
            this.router.navigateByUrl("/");
          }
        }
        else {
          alert("Something Went wrong!");
        }
        this.disablebutton = false;
      }, error => { console.error(error); alert("Something Went wrong!"); this.disablebutton = false; });
    }     
  }

  emailPattern = "^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$";

  isValidFormSubmitted = false;

  

  onFormSubmit(form: NgForm) {
    this.isValidFormSubmitted = false;

    if (form.invalid) {
      return;
    }

    this.isValidFormSubmitted = true;
    form.resetForm();
  }
  onFilechange(event) {
    if (event.target.files.length > 0) {
      let file = event.target.files[0];
      const reader = new FileReader();
      reader.onloadend = () => {
        const base64String = reader.result.toString()
          .replace("data:", "")
          .replace(/^.+,/, "");
        this.photo = base64String;
      };
      reader.readAsDataURL(file);
    }
  }
   
}



