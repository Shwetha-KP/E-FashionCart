import { Component, ɵSWITCH_TEMPLATE_REF_FACTORY__POST_R3__ } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {
  title = 'app';
  logintype: string;
  constructor(private router: Router) {
    this.logintype = localStorage.getItem("logintype");
    switch (this.logintype) {
      case "admin": this.router.navigateByUrl("/adminproduct");break;
      case "customer": this.router.navigateByUrl("/home");break;
      case "agent": this.router.navigateByUrl("/agenthome"); break;
    }
  }
}

