import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SupportOnline } from 'src/app/model/support-online';
import { SupportOnlineService } from 'src/app/services/api/support-online.service';

@Component({
  selector: 'app-support-online-edit',
  templateUrl: './support-online-edit.component.html',
  styleUrls: ['./support-online-edit.component.css']
})
export class SupportOnlineEditComponent {
  support : SupportOnline = new SupportOnline();
  constructor(private supportService: SupportOnlineService, private router: Router, private route: ActivatedRoute) { }

  ngOnInit() : void {
    let token = localStorage.getItem("auth");
    if(token == null){
      this.router.navigate(['/login']);
      alert("Không đủ quyền truy cập");
    }
    else{
      this.route.params.subscribe(params=>{
        let id = params['id'];
        this.supportService.getById(id).then((data)=>{
          this.support = data;
        });
      })
    }
  }

  update(){
    this.supportService.updateSupport(this.support).then((data)=>{
      console.log(data);
      if(data.status === 200){
        alert("Chỉnh sửa thành công!");
        this.router.navigate(['/support']);
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
