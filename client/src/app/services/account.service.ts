import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, ReplaySubject } from 'rxjs';
import { User } from '../models/user';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  baseUrl = 'https://localhost:7029/api/';
  private currentUserSource = new ReplaySubject<User | null>(1);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient) { }

  login(model: any) {
    return this.http.post<User>(`${this.baseUrl}account/login`, model).pipe(
      map((response: User) =>{
        const user = response;
        if(user){
          localStorage.setItem('user', JSON.stringify(user));
          this.currentUserSource.next(user);
        }
      })
    );
  }

  register(model: any){
    return this.http.post<User>(`${this.baseUrl}account/register`, model).pipe(
      map((user : User) => {
        if(user){
          localStorage.setItem('user', JSON.stringify(user));
          this.currentUserSource.next(user);
        }
      })
      
    )
  }

  setCurrentUser(user: User){
    this.currentUserSource.next(user);
  }

  logout(){
    const user: User = {
      username: '',
      token: ''
    }
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
  }
  

}
