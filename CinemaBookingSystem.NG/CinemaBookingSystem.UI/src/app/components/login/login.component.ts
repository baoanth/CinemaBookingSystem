import { Router } from '@angular/router';
import { Account } from './../../models/account-model';
import { Component } from '@angular/core';
import { ApiService } from 'src/app/api.service';
import {HttpClient, HttpClientModule, HttpErrorResponse, HttpParams, HttpResponse, HttpStatusCode} from '@angular/common/http';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  constructor(private apiService: ApiService, private router: Router) { }
  ;
  account: Account = new Account();
  ngOnInit() {
  }

  login(){
    this.apiService.login(this.account).then((data)=>{
      if(data.status === 200){
        localStorage.setItem('token', 'loggedIn');
        this.router.navigate(['/home']);
      }else{
        alert("Tài khoản hoặc mật khẩu không đúng!");
      }
      });
  }
}
