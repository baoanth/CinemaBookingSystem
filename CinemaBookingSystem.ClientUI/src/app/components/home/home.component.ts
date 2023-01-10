import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ApiService } from 'src/app/services/api/api.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {
  constructor(private apiService: ApiService, private router: Router) { }
  ngOnInit() {
    let token = localStorage.getItem("auth");
    if(token == null){
      this.router.navigate(['/login']);
    }
  }

  logout(){
    localStorage.removeItem("auth");
    this.router.navigate(['/login']);
  }
}
