import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class PedidoService {
  private apiUrl =  'http://localhost:5000';

  constructor(private http: HttpClient) {}

  listarPedidos(data: { cursor?: string | null; status?: number | null }): Observable<any> {
    return this.http.post(`${this.apiUrl}/api/pedidos/listar`, data);
  }

  criarPedido(data: { itens: { idProduto: string; quantidade: number }[] }): Observable<any> {
    return this.http.post(`${this.apiUrl}/api/pedidos`, data);
  }

  processarPedido(data: { idPedido: string }): Observable<any> {
    return this.http.put(`${this.apiUrl}/api/pedidos/processar`, data);
  }
}
