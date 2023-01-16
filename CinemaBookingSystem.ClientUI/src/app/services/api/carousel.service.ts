import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Carousel } from 'src/app/model/carousel';

@Injectable({
  providedIn: 'root'
})
export class CarouselService {

  private baseUrl = 'https://localhost:44322/api';
  private authKey = "movienew";
  constructor(private httpClient : HttpClient) { }

  public getCarousels(): Observable<any>{
    const apiUrl = `${this.baseUrl}/carousel/getall`;
    const headers = new HttpHeaders()
      .set('content-type', 'application/json')
      .set('Access-Control-Allow-Origin', '*')
      .set('CBSToken', this.authKey);
    return this.httpClient.get<Carousel[]>(apiUrl,{'headers':headers});
  }

  public async getById(id? : number): Promise<Carousel>{
    const apiUrl = `${this.baseUrl}/carousel/getsingle/${id}`;
    const response = await fetch(apiUrl, {
      method: 'GET',
      mode: 'cors', 
      cache: 'no-cache', 
      credentials: 'same-origin', 
      headers: {
        'Content-Type': 'application/json',
        'Access-Control-Allow-Origin' : '*',
        'CBSToken': this.authKey
      },
      redirect: 'follow', 
      referrerPolicy: 'no-referrer', 
    });
    return response.json();
  }
  
  public async addCarousel(carousel:Carousel): Promise<any>{
    const apiUrl = `${this.baseUrl}/carousel/create`;
    const response = await fetch(apiUrl, {
      method: 'POST',
      mode: 'cors', 
      cache: 'no-cache', 
      credentials: 'same-origin', 
      headers: {
        'Content-Type': 'application/json',
        'Access-Control-Allow-Origin' : '*',
        'CBSToken': this.authKey
      },
      redirect: 'follow', 
      referrerPolicy: 'no-referrer', 
      body: JSON.stringify(carousel)
    });
    console.log(JSON.stringify(carousel));
    return response;
  }

  public async updateCarousel(carousel:Carousel): Promise<any>{
    const apiUrl = `${this.baseUrl}/carousel/update`;
    const response = await fetch(apiUrl, {
      method: 'POST',
      mode: 'cors', 
      cache: 'no-cache', 
      credentials: 'same-origin', 
      headers: {
        'Content-Type': 'application/json',
        'Access-Control-Allow-Origin' : '*',
        'CBSToken': this.authKey
      },
      redirect: 'follow', 
      referrerPolicy: 'no-referrer', 
      body: JSON.stringify(carousel)
    });
    console.log(JSON.stringify(carousel));
    return response;
  }

  public async deleteCarousel(carouselID? : number): Promise<any>{
    const apiUrl = `${this.baseUrl}/carousel/delete/${carouselID}`;
    const response = await fetch(apiUrl, {
      method: 'DELETE',
      mode: 'cors', 
      cache: 'no-cache', 
      credentials: 'same-origin', 
      headers: {
        'Content-Type': 'application/json',
        'Access-Control-Allow-Origin' : '*',
        'CBSToken': this.authKey
      },
      redirect: 'follow', 
      referrerPolicy: 'no-referrer',
    });
    return response;
  }
}
