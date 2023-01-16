import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Account } from 'src/app/model/account';
import { AuthService } from 'src/app/services/auth/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  constructor(private authService: AuthService, private router: Router) {
  }
  ;
  account: Account = new Account();
  token: Account = new Account();
  ngOnInit() {
  }

  login(){
    this.authService.login(this.account).then((response)=>{
      response.json().then(data => ({status: response.status, body: data})).then(data => {
        if(data.status == 200){
          localStorage.setItem('auth',data.body);
          this.router.navigate(['/home']);
          alert("Welcome back, " + this.account.username);
        }else{
          alert("Thông tin xác thực tài khoản của bạn không chính xác!");
        }
        });
      })
      
  }
}
