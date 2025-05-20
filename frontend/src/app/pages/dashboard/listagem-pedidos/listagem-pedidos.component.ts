import { Component, OnInit } from '@angular/core';
import { PedidoService } from '../../../services/pedido.service';
import { CommonModule } from '@angular/common';
import { DialogModule } from 'primeng/dialog';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { RealizarPedidoComponent } from '../realizar-pedido/realizar-pedido.component';

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
    RealizarPedidoComponent
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


  constructor(private pedidoService: PedidoService) {}

  ngOnInit(): void {
    this.carregarMais();
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
        this.temMais = novos.length === 15;

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
  onRowExpand(event: any) {
    const id = event.data.id;
    this.expandedRows = { [id]: true };
    console.log('Linha expandida:', id);
  }

  onRowCollapse(event: any) {
    const id = event.data.id;
    delete this.expandedRows[id];
    console.log('Linha recolhida:', id);
  }

  // Métodos adicionais para expandir/recolher todas as linhas
  expandAll() {
    const expandedRows: { [key: string]: boolean } = {};

    this.pedidos.forEach(pedido => {
      expandedRows[pedido.id] = true;
    });

    this.expandedRows = expandedRows;
  }

  collapseAll() {
    this.expandedRows = {};
  }

  processarPedido(id: string): void {
    this.pedidoService.processarPedido({ idPedido: id }).subscribe({
      next: () => {
        this.pedidos = this.pedidos.map(p =>
          p.id === id ? { ...p, statusAtual: 1 } : p
        );
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
}
