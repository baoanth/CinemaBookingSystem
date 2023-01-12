import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { SupportOnline } from 'src/app/model/support-online';

@Injectable({
  providedIn: 'root'
})
export class SupportOnlineService {
  private baseUrl = 'https://localhost:44322/api';
  private authKey = "movienew";
  constructor(private httpClient : HttpClient) { }

  public getSupports(): Observable<any>{
    const apiUrl = `${this.baseUrl}/support/getall`;
    const headers = new HttpHeaders()
      .set('content-type', 'application/json')
      .set('Access-Control-Allow-Origin', '*')
      .set('CBSToken', this.authKey);
    return this.httpClient.get<SupportOnline[]>(apiUrl,{'headers':headers});
  }

  public async getById(id? : number): Promise<SupportOnline>{
    const apiUrl = `${this.baseUrl}/support/getsingle/${id}`;
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
  
  public async addSupport(support:SupportOnline): Promise<any>{
    const apiUrl = `${this.baseUrl}/support/create`;
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
      body: JSON.stringify(support)
    });
    console.log(JSON.stringify(support));
    return response;
  }

  public async updateSupport(support:SupportOnline): Promise<any>{
    const apiUrl = `${this.baseUrl}/support/update`;
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
      body: JSON.stringify(support)
    });
    console.log(JSON.stringify(support));
    return response;
  }

  public async deleteSupport(id? : number): Promise<any>{
    const apiUrl = `${this.baseUrl}/support/delete/${id}`;
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
