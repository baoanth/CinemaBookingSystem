import { ProvinceService } from 'src/app/services/api/province.service';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Province } from 'src/app/model/province';

@Component({
  selector: 'app-province-list',
  templateUrl: './province-list.component.html',
  styleUrls: ['./province-list.component.css']
})
export class ProvinceListComponent {
  constructor(private provinceService: ProvinceService, private router: Router) { }
  provinceList: Province[] = [];
  ngOnInit() : void {
    let token = localStorage.getItem("auth");
    if(token == null){
      this.router.navigate(['/login']);
    }
    this.provinceService.getProvinces().subscribe((data)=>{
      this.provinceList = data;
    });
  }

  add(){
    this.router.navigate(['/province/add']);
  }

  delete(province : Province){
    let confirmAction = confirm(`Sau khi xóa sẽ không thể hoàn tác, bạn có chắc chắn muốn xóa ${province.provinceName} ra khỏi danh sách?`);
    if(confirmAction){
      let id = province.provinceID;
      this.provinceService.deleteProvince(id).then((data)=>{
        if(data.status === 200){
          alert("Xóa thành công");
          location.reload();
          console.log(province);
        }
      });
    }else{
      alert("Đã hủy thao tác!");
    }
    
  }

  logout(){
    localStorage.removeItem("auth");
    this.router.navigate(['/login']);
  }
}
