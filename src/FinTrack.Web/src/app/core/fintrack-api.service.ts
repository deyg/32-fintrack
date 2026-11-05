import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import {
  Categoria,
  Classificador,
  Consolidado,
  Conta,
  FiltroLancamentos,
  ItemConsolidado,
  Lancamento,
  Natureza,
  Pagina,
} from './fintrack.models';

@Injectable({ providedIn: 'root' })
export class FinTrackApiService {
  private readonly http = inject(HttpClient);

  listarCategorias(): Observable<Pagina<Categoria>> {
    return this.http.get<Pagina<Categoria>>('/categorias', {
      params: { pagina: 1, tamanhoPagina: 100 },
    });
  }

  criarCategoria(nome: string, natureza: Natureza): Observable<Categoria> {
    return this.http.post<Categoria>('/categorias', { nome, natureza });
  }

  renomearCategoria(id: string, nome: string): Observable<Categoria> {
    return this.http.patch<Categoria>(`/categorias/${id}/nome`, { nome });
  }

  alterarEstadoCategoria(id: string, ativa: boolean): Observable<Categoria> {
    const operacao = ativa ? 'ativacao' : 'inativacao';
    return this.http.patch<Categoria>(`/categorias/${id}/${operacao}`, null);
  }

  listarContas(categoriaId = '', ativa = ''): Observable<Pagina<Conta>> {
    let params = new HttpParams().set('pagina', 1).set('tamanhoPagina', 100);
    if (categoriaId) params = params.set('categoriaId', categoriaId);
    if (ativa) params = params.set('ativa', ativa);
    return this.http.get<Pagina<Conta>>('/contas', { params });
  }

  criarConta(
    categoriaId: string,
    nome: string,
    classificador: Classificador | null,
  ): Observable<Conta> {
    return this.http.post<Conta>('/contas', { categoriaId, nome, classificador });
  }

  renomearConta(id: string, nome: string): Observable<Conta> {
    return this.http.patch<Conta>(`/contas/${id}/nome`, { nome });
  }

  alterarEstadoConta(id: string, ativa: boolean): Observable<Conta> {
    const operacao = ativa ? 'ativacao' : 'inativacao';
    return this.http.patch<Conta>(`/contas/${id}/${operacao}`, null);
  }

  definirClassificador(id: string, classificador: Classificador): Observable<Conta> {
    return this.http.patch<Conta>(`/contas/${id}/classificador`, { classificador });
  }

  classificarContaLegada(
    id: string,
    categoriaId: string,
    classificador: Classificador | null,
  ): Observable<Conta> {
    return this.http.patch<Conta>(`/contas/${id}/categoria`, { categoriaId, classificador });
  }

  listarLancamentos(filtro: FiltroLancamentos = {}): Observable<Pagina<Lancamento>> {
    let params = new HttpParams().set('pagina', 1).set('tamanhoPagina', 100);
    for (const [chave, valor] of Object.entries(filtro)) {
      if (valor) params = params.set(chave, valor);
    }
    return this.http.get<Pagina<Lancamento>>('/lancamentos', { params });
  }

  registrarLancamento(
    contaId: string,
    valor: number,
    dataRealizacao: string,
    descricao: string,
  ): Observable<Lancamento> {
    return this.http.post<Lancamento>('/lancamentos', {
      contaId,
      valor,
      dataRealizacao,
      descricao: descricao || null,
    });
  }

  cancelarLancamento(id: string, motivo: string): Observable<Lancamento> {
    return this.http.post<Lancamento>(`/lancamentos/${id}/cancelamento`, {
      motivo: motivo || null,
    });
  }

  obterConsolidado(dataInicial: string, dataFinal: string): Observable<Consolidado> {
    return this.http.get<Consolidado>('/consolidados', {
      params: { dataInicial, dataFinal },
    });
  }

  obterAgrupamento(
    tipo: 'conta' | 'categoria' | 'classificador',
    dataInicial: string,
    dataFinal: string,
  ): Observable<ItemConsolidado[]> {
    return this.http.get<ItemConsolidado[]>(`/consolidados/por-${tipo}`, {
      params: { dataInicial, dataFinal },
    });
  }
}
