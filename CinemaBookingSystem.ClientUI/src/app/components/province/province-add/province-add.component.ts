import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Province } from 'src/app/model/province';
import { ApiService } from 'src/app/services/api/api.service';

@Component({
  selector: 'app-province-add',
  templateUrl: './province-add.component.html',
  styleUrls: ['./province-add.component.css']
})
export class ProvinceAddComponent {
  province : Province = new Province();
  constructor(private apiService: ApiService, private router: Router) { }
  ngOnInit() : void {
    let token = localStorage.getItem("auth");
    if(token == null){
      this.router.navigate(['/login']);
    }
  }

  addProvince(){
    this.apiService.addProvince(this.province).then((data)=>{
      console.log(data);
      if(data.status === 201){
        alert("Thêm thành công!");
        this.router.navigate(['/province']);
      }else{
        alert("Không hợp lệ!");
      }
      });
  }

  logout(){
    localStorage.removeItem("auth");
    this.router.navigate(['/login']);
  }
}
