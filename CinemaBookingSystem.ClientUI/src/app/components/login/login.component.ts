import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Account } from 'src/app/model/account';
import { ApiService } from 'src/app/services/api/api.service';

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
        localStorage.setItem('auth', 'loggedIn');
        this.router.navigate(['/home']);
      }else{
        alert("Tài khoản hoặc mật khẩu không đúng!");
      }
      });
  }
}
