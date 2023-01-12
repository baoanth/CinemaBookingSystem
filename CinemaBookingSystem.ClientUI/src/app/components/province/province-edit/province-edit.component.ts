import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Province } from 'src/app/model/province';
import { ProvinceService } from 'src/app/services/api/province.service';

@Component({
  selector: 'app-province-edit',
  templateUrl: './province-edit.component.html',
  styleUrls: ['./province-edit.component.css']
})
export class ProvinceEditComponent {
  province : Province = new Province();
  constructor(private provinceService: ProvinceService, private router: Router, private route: ActivatedRoute) { }

  ngOnInit() : void {
    let token = localStorage.getItem("auth");
    if(token == null){
      this.router.navigate(['/login']);
    }
    else{
      this.route.params.subscribe(params=>{
        let id = params['id'];
        this.provinceService.getById(id).then((data)=>{
          this.province = data;
        });
      })
    }
  }

  update(){
    this.provinceService.updateProvince(this.province).then((data)=>{
      console.log(data);
      if(data.status === 200){
        alert("Chỉnh sửa thành công!");
        this.router.navigate(['/province']);
      }else{
        alert("Thông tin trống hoặc không hợp lệ!");
      }
      });
  }

  logout(){
    localStorage.removeItem("auth");
    this.router.navigate(['/login']);
  }
}
