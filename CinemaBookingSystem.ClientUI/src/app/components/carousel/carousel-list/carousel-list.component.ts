import { Carousel } from './../../../model/carousel';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { CarouselService } from 'src/app/services/api/carousel.service';

@Component({
  selector: 'app-carousel-list',
  templateUrl: './carousel-list.component.html',
  styleUrls: ['./carousel-list.component.css']
})
export class CarouselListComponent {
  constructor(private carouselService: CarouselService, private router: Router) { }
  carouselList: Carousel[] = [];
  ngOnInit() : void {
    let token = localStorage.getItem("auth");
    if(token == null){
      this.router.navigate(['/login']);
      alert("Không đủ quyền truy cập");
    }
    this.carouselService.getCarousels().subscribe((data)=>{
      this.carouselList = data;
    });
  }

  delete(carousel : Carousel){
    let confirmAction = prompt(`Sau khi xóa sẽ không thể hoàn tác, hãy nhập ${carousel.carouselName} để xóa ra khỏi danh sách?`);
    if(confirmAction == `${carousel.carouselName}`){
      let id = carousel.carouselID;
      this.carouselService.deleteCarousel(id).then((data)=>{
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
