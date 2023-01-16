import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Province } from 'src/app/model/province';
import { ProvinceService } from 'src/app/services/api/province.service';

@Component({
  selector: 'app-province-add',
  templateUrl: './province-add.component.html',
  styleUrls: ['./province-add.component.css']
})
export class ProvinceAddComponent {
  province : Province = new Province();
  constructor(private provinceService: ProvinceService, private router: Router) { }
  ngOnInit() : void {
    let token = localStorage.getItem("auth");
    if(token == null){
      this.router.navigate(['/login']);
      alert("Không đủ quyền truy cập");
    }
  }

  add(){
    this.provinceService.addProvince(this.province).then((data)=>{
      console.log(data);
      if(data.status === 201){
        alert("Thêm thành công!");
        this.router.navigate(['/province']);
      }else{
        alert("Hãy nhập đầy đủ thông tin!");
      }
      });
  }

  logout(){
    localStorage.removeItem("auth");
    this.router.navigate(['/login']);
  }
}
