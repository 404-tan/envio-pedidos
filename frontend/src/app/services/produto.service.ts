import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class ProdutoService {
  private apiUrl = 'http://localhost:5000';

  constructor(private http: HttpClient) {}

  listarProdutos(): Observable<any> {
    return this.http.get(`${this.apiUrl}/api/produtos`);
  }
}
