import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Config } from '../data/config';
import { environment } from 'src/environments/environment';
import { Result } from '../data/result';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  constructor(private http: HttpClient) { 
  }

  getGuildsCount() : Observable<Result> {

    let headers = new HttpHeaders().set("accept", "*/*");

    return this.http.get<Result>(environment.baseUri + 'TotalGuildsCount', { headers: headers });
  }

  getSubredditsCount() : Observable<Result> {

    let headers = new HttpHeaders().set("accept", "*/*");

    return this.http.get<Result>(environment.baseUri + 'TotalSubredditsCount', { headers: headers });
  }
}