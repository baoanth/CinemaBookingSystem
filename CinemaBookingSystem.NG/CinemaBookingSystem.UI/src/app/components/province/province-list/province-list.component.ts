import { Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../../api.service';
import { Province } from '../../../models/province-model';
@Component({
  selector: 'app-province-list',
  templateUrl: './province-list.component.html',
  styleUrls: ['./province-list.component.css']
})
export class ProvinceListComponent implements OnInit {
  constructor(private apiService: ApiService, private router: Router) { }
  provinceList: Province[] = [];
  ngOnInit() {
    let token = localStorage.getItem("token");
    if(token != "loggedIn"){
      this.router.navigate(['/login']);
    }
    this.apiService.getProvinces().subscribe((data)=>{
      console.log(data);
      this.provinceList = data;
    });
  }
}
