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
  ngOnInit() {
  }

  login(){
    this.authService.login(this.account).then((data)=>{
      if(data.username != ""){
        localStorage.setItem('auth', data.username);
        this.router.navigate(['/home']);
      }else{
        alert("Thông tin xác thực tài khoản của bạn không chính xác!");
      }
      });
  }
}
