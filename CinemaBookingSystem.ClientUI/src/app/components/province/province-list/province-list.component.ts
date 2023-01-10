import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Province } from 'src/app/model/province';
import { ApiService } from 'src/app/services/api/api.service';

@Component({
  selector: 'app-province-list',
  templateUrl: './province-list.component.html',
  styleUrls: ['./province-list.component.css']
})
export class ProvinceListComponent {
  constructor(private apiService: ApiService, private router: Router) { }
  provinceList: Province[] = [];
  ngOnInit() : void {
    let token = localStorage.getItem("auth");
    if(token == null){
      this.router.navigate(['/login']);
    }
    this.apiService.getProvinces().subscribe((data)=>{
      this.provinceList = data;
    });
  }

  add(){
    this.router.navigate(['/province-add']);
  }

  logout(){
    localStorage.removeItem("auth");
    this.router.navigate(['/login']);
  }
}
