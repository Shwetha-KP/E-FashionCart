import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Local } from 'protractor/built/driverProviders';

@Component({
  selector: 'app-logout',
  templateUrl: './logout.component.html',
  styleUrls: ['./logout.component.css']
})
export class LogoutComponent implements OnInit {

  constructor(private router: Router) {
    localStorage.clear();
    router.navigateByUrl("/");
  }

  ngOnInit() {
  }

}
