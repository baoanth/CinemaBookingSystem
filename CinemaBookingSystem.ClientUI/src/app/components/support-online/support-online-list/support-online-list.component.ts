import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { SupportOnline } from 'src/app/model/support-online';
import { SupportOnlineService } from 'src/app/services/api/support-online.service';

@Component({
  selector: 'app-support-online-list',
  templateUrl: './support-online-list.component.html',
  styleUrls: ['./support-online-list.component.css']
})
export class SupportOnlineListComponent {
  constructor(private supportService: SupportOnlineService, private router: Router) { }
  supportList: SupportOnline[] = [];
  ngOnInit() : void {
    let token = localStorage.getItem("auth");
    if(token == null){
      this.router.navigate(['/login']);
      alert("Không đủ quyền truy cập");
    }
    this.supportService.getSupports().subscribe((data)=>{
      this.supportList = data;
    });
  }

  add(){
    this.router.navigate(['/support/add']);
  }

  delete(support : SupportOnline){
    let confirmAction = prompt(`Sau khi xóa sẽ không thể hoàn tác, hãy nhập ${support.supportName} để xóa ra khỏi danh sách?`);
    if(confirmAction == `${support.supportName}`){
      let id = support.supportID;
      this.supportService.deleteSupport(id).then((data)=>{
        if(data.status === 200){
          alert("Xóa thành công");
          location.reload();
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
