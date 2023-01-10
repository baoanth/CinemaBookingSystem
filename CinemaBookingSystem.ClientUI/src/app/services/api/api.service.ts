import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { Account } from 'src/app/model/account';
import { Province } from 'src/app/model/province';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private baseUrl = 'https://localhost:44322/api';
  private authKey = "movienew";
  constructor(private httpClient : HttpClient) { }

  public async login(account:Account){
    const apiUrl = `${this.baseUrl}/account/systemlogin`;
    const response = await fetch(apiUrl, {
      method: 'POST',
      mode: 'cors', 
      cache: 'no-cache', 
      credentials: 'same-origin', 
      headers: {
        'Content-Type': 'application/json',
        'Access-Control-Allow-Origin' : '*',
        'CBSToken': 'movienew'
      },
      redirect: 'follow', 
      referrerPolicy: 'no-referrer', 
      body: JSON.stringify(account)
    });
    return response;
  }

  public getProvinces(): Observable<any>{
    const apiUrl = `${this.baseUrl}/province/getall`;
    const headers = new HttpHeaders()
      .set('content-type', 'application/json')
      .set('Access-Control-Allow-Origin', '*')
      .set('CBSToken', 'movienew');
    return this.httpClient.get<Province[]>(apiUrl,{'headers':headers});
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
        'CBSToken': 'movienew'
      },
      redirect: 'follow', 
      referrerPolicy: 'no-referrer', 
      body: JSON.stringify(province)
    });
    console.log(JSON.stringify(province));
    return response;
  }
}