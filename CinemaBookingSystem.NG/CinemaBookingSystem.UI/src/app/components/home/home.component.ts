import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ApiService } from 'src/app/api.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {
  constructor(private apiService: ApiService, private router: Router) { }
  ngOnInit() {
    let token = localStorage.getItem("token");
    if(token != "loggedIn"){
      this.router.navigate(['/login']);
    }
  }

  logout(){
    localStorage.removeItem("token");
    this.router.navigate(['/login']);
  }
}
