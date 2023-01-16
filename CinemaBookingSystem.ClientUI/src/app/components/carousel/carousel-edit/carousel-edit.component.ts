import { Carousel } from 'src/app/model/carousel';
import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CarouselService } from 'src/app/services/api/carousel.service';

@Component({
  selector: 'app-carousel-edit',
  templateUrl: './carousel-edit.component.html',
  styleUrls: ['./carousel-edit.component.css']
})
export class CarouselEditComponent {
  carousel : Carousel = new Carousel();
  displayOrder? : number;
  constructor(private carouselService: CarouselService, private router: Router, private route: ActivatedRoute) { }

  ngOnInit() : void {
    let token = localStorage.getItem("auth");
    if(token == null){
      this.router.navigate(['/login']);
      alert("Không đủ quyền truy cập");
    }
    else{
      this.route.params.subscribe(params=>{
        let id = params['id'];
        this.carouselService.getById(id).then((data)=>{
          this.carousel = data;
          this.displayOrder = this.carousel.displayOrder;
        });
      })
    }
  }

  update(){
    if(this.displayOrder == this.carousel.displayOrder){
      this.updateFunc();
      return;
    }else{
      this.carouselService.getCarousels().subscribe((data : Carousel[])=>{
        data.forEach(c => {
          if(c.displayOrder == this.carousel.displayOrder){
            alert("Vị trí đã có hình ảnh hiển thị!");
          }else{
            this.updateFunc();
            return;
          }
        });
      }
    )}
  }

  updateFunc(){
    this.carouselService.updateCarousel(this.carousel).then((data)=>{
      console.log(data);
      if(data.status === 200){
        alert("Chỉnh sửa thành công!");
        this.router.navigate(['/carousel']);
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
