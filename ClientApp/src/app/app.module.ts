import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent} from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { LogoutComponent } from './logout/logout.component';
import { AngularFontAwesomeModule } from 'angular-font-awesome';
import { ProductComponent } from './product/product.component';
import { AdminproductComponent,FilterGender } from './adminproduct/adminproduct.component';
import { PatternComponent } from './pattern/pattern.component';
import { OrderComponent } from './order/order.component';
import { AdminComponent } from './admin/admin.component';
import { CartComponent } from './cart/cart.component';
import { NgxSplideModule } from 'ngx-splide';
import { PaymentComponent } from './payment/payment.component';
import { TrackingorderComponent } from './trackingorder/trackingorder.component';
import { FAQComponent } from './faq/faq.component';
import { AgentHomeComponent } from './agent-home/agent-home.component';


@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    FilterGender,
    CounterComponent,
    FetchDataComponent,
    LoginComponent,
    RegisterComponent,
    LogoutComponent,
    ProductComponent,
    AdminproductComponent,
    PatternComponent,
    OrderComponent,
    AdminComponent,
    CartComponent,
    PaymentComponent,
    TrackingorderComponent,
    FAQComponent,
    AgentHomeComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    NgxSplideModule,
    AngularFontAwesomeModule,
    RouterModule.forRoot([
      { path: '', component: LoginComponent, pathMatch: 'full' },
      { path: 'home', component: HomeComponent },
      { path: 'counter', component: CounterComponent },
      { path: 'fetch-data', component: FetchDataComponent },
      { path: 'register', component: RegisterComponent },
      { path: 'logout', component: LogoutComponent },
      { path: 'adminproduct', component: AdminproductComponent },
      { path: 'admin', component: AdminComponent },
      { path: 'product', component: ProductComponent },
      { path: 'order/:orderId', component: OrderComponent },
      { path: 'patterns', component: PatternComponent },
      { path: 'patterns/:productid', component: PatternComponent },
      { path: 'cart', component: CartComponent }, 
      { path: 'myorder', component: TrackingorderComponent },
      { path: 'faq', component: FAQComponent },
      { path: 'agenthome', component: AgentHomeComponent }
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
