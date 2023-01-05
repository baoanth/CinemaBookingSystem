import { Province } from './models/province-model';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Account } from './models/account-model';
import { Observable } from 'rxjs/internal/Observable';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private baseUrl = 'https://localhost:44322/api';
  private authKey = "movienew";
  constructor(private httpClient : HttpClient) { }

  public async login(account:Account){
    const apiUrl = `${this.baseUrl}/account/systemlogin`;
    // const headers = new HttpHeaders()
    //   .set('content-type', 'application/json')
    //   .set('Access-Control-Allow-Origin', '*')
    //   .set('CBSToken', 'movienew');
    // console.log(headers);
    // const body= JSON.stringify(account);
    // console.log(body)
    // return this.httpClient.post(apiUrl,body,{'headers':headers}); 
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
    console.log(headers);
    return this.httpClient.get<Province[]>(apiUrl,{'headers':headers});
  }
}
