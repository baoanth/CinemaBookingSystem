
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Carousel } from 'src/app/model/carousel';
import { CarouselService } from 'src/app/services/api/carousel.service';

@Component({
  selector: 'app-carousel-add',
  templateUrl: './carousel-add.component.html',
  styleUrls: ['./carousel-add.component.css']
})
export class CarouselAddComponent {
  carousel : Carousel = new Carousel();
  constructor(private carouselService: CarouselService, private router: Router) { }
  ngOnInit() : void {
    let token = localStorage.getItem("auth");
    if(token == null){
      this.router.navigate(['/login']);
      alert("Không đủ quyền truy cập");
    }
  }

add(){
  this.carouselService.getCarousels().subscribe((data : Carousel[])=>{
    data.forEach(c => {
      if(c.displayOrder == this.carousel.displayOrder){
        alert("Vị trí đã có hình ảnh hiển thị!");
      }else{
        this.carouselService.addCarousel(this.carousel).then((data)=>{
          if(data.status === 201){
            alert("Thêm thành công!");
            this.router.navigate(['/carousel']);
          }else{
            alert("Thiếu thông tin hoặc thông tin không hợp lệ!");
          }
          });
      }
    });
  }
)}

  logout(){
    localStorage.removeItem("auth");
    this.router.navigate(['/login']);
  }
}
