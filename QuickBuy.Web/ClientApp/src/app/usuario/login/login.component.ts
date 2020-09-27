import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { Usuario } from "../../modelo/usuario";
import { UsuarioServico } from "../../servicos/usuario/usuario.servico";

@Component({
  selector: "app-login",
  templateUrl: "./login.component.html",
  styleUrls: ["./login.component.css"]
})

export class LoginComponent implements OnInit {
  public usuario;
  //public usuarioAutenticado: boolean;
  //public usuarios = ["usuario1", "usuario2", "usuario3", "usuario4", "usuario5"];
  public returnUrl: string;
  public mensagem: string;

  constructor(private router: Router, private activatedRouter: ActivatedRoute,
    private usuarioServico: UsuarioServico ) {
    // usar apenas para obter referencia a classes que podem ser injetadas
  }

  ngOnInit(): void {    // iniciando o ciclo de vida do componente // OnInit é chamado DEPOIS do construtor
    this.returnUrl = this.activatedRouter.snapshot.queryParams['returnUrl'];
    this.usuario = new Usuario();
  }

  entrar() {

    this.usuarioServico.verificarUsuario(this.usuario)
      .subscribe(
        data => {
          //console.log(data);

          var usuarioRetorno: Usuario;
          usuarioRetorno = data;
          sessionStorage.setItem("usuario-autenticado", "1");
          sessionStorage.setItem("email-usuario", usuarioRetorno.email);

          if (this.returnUrl == null) {
            this.router.navigate(['/']);
          } else {
            this.router.navigate([this.returnUrl]);
          }

        },
        err => {
          console.log(err.error);
          this.mensagem = err.error;
        }
      );

    //if (this.usuario.email == "marcio@teste.com" && this.usuario.senha == "teste") {
    ////  this.usuarioAutenticado = true;

    //  sessionStorage.setItem("usuario-autenticado", "1");
    //  this.router.navigate([this.returnUrl]);
    //}
  }

}
