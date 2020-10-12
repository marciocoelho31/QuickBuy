import { Component, OnInit } from "@angular/core"
import { error } from "util";
import { Produto } from "../modelo/produto";
import { ProdutoServico } from "../servicos/produto/produto.servico";

@Component({
  selector: "app-produto",   // onde será renderizado o componente (qual tag)
  templateUrl: "./produto.component.html",
  styleUrls: ["./produto.component.css"]
})

export class ProdutoComponent implements OnInit {
  public produto: Produto;
  public arquivoSelecionado: File;
  public ativarSpinner: boolean;

  constructor(private produtoServico: ProdutoServico) {

  }

  ngOnInit(): void {
    this.produto = new Produto();
  }

  public inputChange(files: FileList) {
    this.arquivoSelecionado = files.item(0);
    this.ativarSpinner = true;
    this.produtoServico.enviarArquivo(this.arquivoSelecionado)
      .subscribe(
        nomeArquivo => {
          this.produto.nomeArquivo = nomeArquivo;
          console.log(nomeArquivo);
          this.ativarSpinner = false;
        }, e => {
          console.log(e.error);
          this.ativarSpinner = false;
        });
  }

  public cadastrar() {
    //this.produtoServico.cadastrar(this.produto)
    //  .subscribe(
    //    produtoJson => {
    //      console.log(produtoJson);
    //    },
    //    e => {
    //      console.log(e.error);
    //    }
    //  );
  }
}
