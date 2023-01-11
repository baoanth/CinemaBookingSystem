import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Account } from 'src/app/model/account';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
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
        'CBSToken': this.authKey
      },
      redirect: 'follow', 
      referrerPolicy: 'no-referrer', 
      body: JSON.stringify(account)
    });
    return response.json();
  }
}