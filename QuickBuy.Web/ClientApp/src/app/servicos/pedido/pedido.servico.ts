import { Injectable, Inject, OnInit } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable } from "rxjs";   // (para programação reativa)
import { Pedido } from "../../modelo/pedido";

@Injectable({
  providedIn: "root"
})
export class PedidoServico implements OnInit {
  private baseURL: string;

  ngOnInit(): void {

  }

  constructor(private http: HttpClient, @Inject('BASE_URL') baseurl: string) {
    this.baseURL = baseurl;
  }

  get headers(): HttpHeaders {
    return new HttpHeaders().set('content-type', 'application/json');
  }

  public efetivarCompra(pedido: Pedido): Observable<number> {
    return this.http.post<number>(this.baseURL + "api/pedido", JSON.stringify(pedido), { headers: this.headers });
  }

}
