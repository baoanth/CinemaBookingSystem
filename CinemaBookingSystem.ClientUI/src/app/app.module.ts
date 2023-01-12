import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { ProvinceListComponent } from './components/province/province-list/province-list.component';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { ProvinceAddComponent } from './components/province/province-add/province-add.component';
import { NgxPaginationModule } from 'ngx-pagination';
import { ProvinceEditComponent } from './components/province/province-edit/province-edit.component';
import { PageNotFoundComponent } from './components/page-not-found/page-not-found.component';
import { SupportOnlineListComponent } from './components/support-online/support-online-list/support-online-list.component';
import { SupportOnlineAddComponent } from './components/support-online/support-online-add/support-online-add.component';
import { SupportOnlineEditComponent } from './components/support-online/support-online-edit/support-online-edit.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    LoginComponent,
    ProvinceListComponent,
    ProvinceAddComponent,
    ProvinceEditComponent,
    PageNotFoundComponent,
    SupportOnlineListComponent,
    SupportOnlineAddComponent,
    SupportOnlineEditComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    NgxPaginationModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
