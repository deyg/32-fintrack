export type Natureza = 'RECEITA' | 'DESPESA';
export type Classificador = 'OF' | 'OV' | 'NOF' | 'NOV';
export type SituacaoLancamento = 'REALIZADO' | 'CANCELADO';

export interface Pagina<T> {
  itens: T[];
  pagina: number;
  tamanhoPagina: number;
  total: number;
}

export interface Categoria {
  id: string;
  nome: string;
  natureza: Natureza;
  ativa: boolean;
}

export interface Conta {
  id: string;
  categoriaId: string | null;
  nome: string;
  classificador: Classificador | null;
  ativa: boolean;
}

export interface Lancamento {
  id: string;
  contaId: string;
  valor: number;
  dataRealizacao: string;
  descricao: string | null;
  situacao: SituacaoLancamento;
  dataCancelamento: string | null;
  motivoCancelamento: string | null;
}

export interface ItemConsolidado {
  id: string | null;
  nome: string;
  total: number;
}

export interface Consolidado {
  receitas: number;
  despesas: number;
  resultado: number;
  dataInicial: string;
  dataFinal: string;
  porConta: ItemConsolidado[];
  porCategoria: ItemConsolidado[];
  porClassificador: ItemConsolidado[];
}

export interface FiltroLancamentos {
  dataInicial?: string;
  dataFinal?: string;
  categoriaId?: string;
  contaId?: string;
  classificador?: Classificador | '';
  situacao?: SituacaoLancamento | '';
}

export interface ProblemDetails {
  title?: string;
  detail?: string;
  status?: number;
  code?: string;
  errors?: Record<string, string[]>;
}
