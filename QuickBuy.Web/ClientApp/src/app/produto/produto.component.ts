import { Component } from "@angular/core"

@Component({
  selector: "app-produto",   // onde será renderizado o componente (qual tag)
  template: "<html><body>{{ obterNome() }}</body></html>"
})
export class ProdutoComponent {
  public nome: string;
  public liberadoParaVenda: boolean;

  public obterNome(): string {
    return "Samsung";
  }
}
