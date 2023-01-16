import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { SupportOnline } from 'src/app/model/support-online';
import { SupportOnlineService } from 'src/app/services/api/support-online.service';

@Component({
  selector: 'app-support-online-add',
  templateUrl: './support-online-add.component.html',
  styleUrls: ['./support-online-add.component.css']
})
export class SupportOnlineAddComponent {
  support : SupportOnline = new SupportOnline();
  constructor(private supportService: SupportOnlineService, private router: Router) { }
  ngOnInit() : void {
    let token = localStorage.getItem("auth");
    if(token == null){
      this.router.navigate(['/login']);
      alert("Không đủ quyền truy cập");
    }
  }

  add(){
    this.supportService.addSupport(this.support).then((data)=>{
      console.log(data);
      if(data.status === 201){
        alert("Thêm thành công!");
        this.router.navigate(['/support']);
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
