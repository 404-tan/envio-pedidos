import { Component, OnInit } from '@angular/core';
import { PedidoService } from '../../../services/pedido.service';
import { CommonModule } from '@angular/common';
import { DialogModule } from 'primeng/dialog';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { RealizarPedidoComponent } from '../realizar-pedido/realizar-pedido.component';
import { MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';
import { AuthService } from '../../../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-listagem-pedidos',
  templateUrl: './listagem-pedidos.component.html',
  styleUrls: ['./listagem-pedidos.component.css'],
  standalone: true,
  imports: [
    CommonModule,
    TableModule,
    ButtonModule,
    DialogModule,
    RealizarPedidoComponent,
    ToastModule
  ]
})
export class ListagemPedidosComponent implements OnInit {
  pedidos: any[] = [];
  carregando = false;
  cursor: string | null = null;
  mostrarModal = false;
  temMais = true;
  expandedRows: { [key: string]: boolean } = {};
  filtroStatus: number | null = null;
  perfil: { id: string,nome:string,isAdmin:boolean } | null = null;

  constructor(private pedidoService: PedidoService,private authService: AuthService,private messageService : MessageService
    ,private router: Router
  ) {}

  ngOnInit(): void {
    this.carregarMais();
  }
  carregarPerfil(){
    this.authService.perfil().subscribe({
      next: (res) => {
        this.perfil = res;
      },
      error: () => {
        this.messageService.add({
          severity: 'error',
          summary: 'Erro',
          detail: 'Não foi possível carregar o perfil.',
          life: 4000
        });
      }
    });
  }

  carregarMais(): void {
    if (this.carregando || !this.temMais) return;

    this.carregando = true;

    this.pedidoService.listarPedidos({
      cursor: this.cursor,
      status: this.filtroStatus
    }).subscribe({
      next: (res) => {
        const novos = res || [];
        this.pedidos.push(...novos);

        const ultimoPedido = novos[novos.length - 1];
        this.cursor = ultimoPedido?.dataCriacao ?? null;
        this.temMais = novos.length === 10;

        this.carregando = false;
      },
      error: () => {
        this.carregando = false;
      }
    });
  }
  setFiltro(status: number | null) {
    this.filtroStatus = status;
    this.pedidos = [];
    this.cursor = null;
    this.temMais = true;
    this.carregarMais();
  }

  processarPedido(id: string): void {
    this.pedidoService.enfileirarPedido({ idPedido: id }).subscribe({
      next: () => {
        this.pedidos = this.pedidos.map(p =>
          p.id === id ? { ...p, statusAtual: 1 } : p
        );

        this.messageService.add({
          severity: 'info',
          summary: 'Pedido enviado',
          detail: 'Seu pedido está sendo processado. Aguarde...',
          life: 4000
        });
      },
      error: () => {
        this.messageService.add({
          severity: 'error',
          summary: 'Erro',
          detail: 'Não foi possível enfileirar o pedido.',
          life: 4000
        });
      }
    });
  }

  abrirModal(): void {
    this.mostrarModal = true;
  }

  onPedidoCriado(): void {
    this.mostrarModal = false;
    this.pedidos = [];
    this.cursor = null;
    this.temMais = true;
    this.carregarMais();
  }
  logout(): void {
    localStorage.removeItem('token');
    sessionStorage.removeItem('token');
    this.router.navigate(['/login']);
  }
}
