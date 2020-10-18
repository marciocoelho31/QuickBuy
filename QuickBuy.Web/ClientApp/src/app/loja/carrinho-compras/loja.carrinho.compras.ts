import { Produto } from "../../modelo/produto";

export class LojaCarrinhoCompras {
  public produtos: Produto[] = [];

  public adicionarProduto(produto: Produto) {
    var produtoLocalStorage = localStorage.getItem("produtoLocalStorage");
    if (!produtoLocalStorage) {
      // se nao existir nada dentro do localStorage
      this.produtos.push(produto);
    }
    else {
      // se ja existir pelo menos um unico item armazenado na sessão (localStorage)
      this.produtos = JSON.parse(produtoLocalStorage);
      this.produtos.push(produto);
    }

    localStorage.setItem("produtoLocalStorage", JSON.stringify(this.produtos));

  }

  public obterProdutos(): Produto[] {    // tipando mas nao é obrigatório nesse caso
    var produtoLocalStorage = localStorage.getItem("produtoLocalStorage");
    if (produtoLocalStorage) {
      return JSON.parse(produtoLocalStorage);
    }
    return this.produtos;
  }

  public removerProduto(produto: Produto) {
    var produtoLocalStorage = localStorage.getItem("produtoLocalStorage");
    if (produtoLocalStorage) {
      this.produtos = JSON.parse(produtoLocalStorage);
      this.produtos = this.produtos.filter(p => p.id != produto.id);
      localStorage.setItem("produtoLocalStorage", JSON.stringify(this.produtos));
    }
  }

  public atualizar(produtos: Produto[]) {
    localStorage.setItem("produtoLocalStorage", JSON.stringify(produtos));
  }

  public temItensCarrinhoCompras(): boolean {
    var itens = this.obterProdutos();
    return (itens.length > 0);
  }

}
