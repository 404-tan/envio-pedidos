import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ProdutoService } from '../../../services/produto.service';
import { PedidoService } from '../../../services/pedido.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-realizar-pedido',
  templateUrl: './realizar-pedido.component.html',
  styleUrls: ['./realizar-pedido.component.css'],
  imports:[CommonModule, ReactiveFormsModule]
})
export class RealizarPedidoComponent implements OnInit {
  produtos: any[] = [];
  pedidoForm: FormGroup;

  itensPedido: {
    idProduto: string;
    nomeProduto: string;
    precoUnitario: number;
    quantidade: number;
  }[] = [];

  constructor(
    private fb: FormBuilder,
    private produtoService: ProdutoService,
    private pedidoService: PedidoService
  ) {
    this.pedidoForm = this.fb.group({
      produto: ['', Validators.required],
      quantidade: [1, [Validators.required, Validators.min(1)]]
    });
  }

  ngOnInit(): void {
    this.produtoService.listarProdutos().subscribe({
      next: (res) => (this.produtos = res),
      error: () => console.error('Erro ao carregar produtos.')
    });
  }

adicionarItem() {
  if (this.pedidoForm.invalid) return;

  const produtoId = this.pedidoForm.value.produto;
  const quantidade = this.pedidoForm.value.quantidade;
  const produto = this.produtos.find(p => p.id === produtoId);

  if (!produto) return;

  const itemExistente = this.itensPedido.find(item => item.idProduto === produtoId);

  if (itemExistente) {
    itemExistente.quantidade += quantidade;
  } else {

    this.itensPedido.push({
      idProduto: produto.id,
      nomeProduto: produto.nome,
      precoUnitario: produto.precoAtual,
      quantidade
    });
  }

  this.pedidoForm.reset({ quantidade: 1 });
}


  removerItem(index: number) {
    this.itensPedido.splice(index, 1);
  }

  enviarPedido() {
    if (this.itensPedido.length === 0) return;

    this.pedidoService.criarPedido({ itens: this.itensPedido }).subscribe({
      next: () => {
        alert('Pedido realizado com sucesso!');
        this.itensPedido = [];
      },
      error: () => alert('Erro ao enviar pedido.')
    });
  }

  get totalPedido(): number {
    return this.itensPedido.reduce((total, item) => total + item.precoUnitario * item.quantidade, 0);
  }
}
