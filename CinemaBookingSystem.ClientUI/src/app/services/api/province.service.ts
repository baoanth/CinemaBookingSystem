import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { Account } from 'src/app/model/account';
import { Province } from 'src/app/model/province';

@Injectable({
  providedIn: 'root'
})
export class ProvinceService {
  private baseUrl = 'https://localhost:44322/api';
  private authKey = "movienew";
  constructor(private httpClient : HttpClient) { }

  public getProvinces(): Observable<any>{
    const apiUrl = `${this.baseUrl}/province/getall`;
    const headers = new HttpHeaders()
      .set('content-type', 'application/json')
      .set('Access-Control-Allow-Origin', '*')
      .set('CBSToken', this.authKey);
    return this.httpClient.get<Province[]>(apiUrl,{'headers':headers});
  }

  public async getProvinceById(id? : number): Promise<Province>{
    const apiUrl = `${this.baseUrl}/province/getsingle/${id}`;
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
  
  public async addProvince(province:Province): Promise<any>{
    const apiUrl = `${this.baseUrl}/province/create`;
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
      body: JSON.stringify(province)
    });
    console.log(JSON.stringify(province));
    return response;
  }

  public async updateProvince(province:Province): Promise<any>{
    const apiUrl = `${this.baseUrl}/province/update`;
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
      body: JSON.stringify(province)
    });
    console.log(JSON.stringify(province));
    return response;
  }

  public async deleteProvince(provinceId? : number): Promise<any>{
    const apiUrl = `${this.baseUrl}/province/delete/${provinceId}`;
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