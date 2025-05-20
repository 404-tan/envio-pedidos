import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private apiUrl =  'http://localhost:5000';

  constructor(private http: HttpClient) {}

  login(credentials: { email: string; senhaDigitada: string }): Observable<any> {
    return this.http.post(`${this.apiUrl}/api/login`, credentials);
  }

  registrar(data: { nomeCompleto: string; email: string; senhaDigitada: string }): Observable<any> {
    return this.http.post(`${this.apiUrl}/api/registrar`, data);
  }

  perfil(): Observable<any> {
    return this.http.get(`${this.apiUrl}/api/perfil`);
  }
}
