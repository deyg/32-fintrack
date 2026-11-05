import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit, computed, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { firstValueFrom } from 'rxjs';
import { FinTrackApiService } from './core/fintrack-api.service';
import {
  Categoria,
  Classificador,
  Consolidado,
  Conta,
  FiltroLancamentos,
  Lancamento,
  Natureza,
  ProblemDetails,
} from './core/fintrack.models';

type Secao = 'visao-geral' | 'categorias' | 'contas' | 'lancamentos';
type TomMensagem = 'sucesso' | 'erro';

@Component({
  selector: 'app-root',
  imports: [CommonModule, FormsModule],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class App implements OnInit {
  private readonly api = inject(FinTrackApiService);

  protected readonly secoes: { id: Secao; rotulo: string; legenda: string }[] = [
    { id: 'visao-geral', rotulo: 'Visão geral', legenda: 'Resumo financeiro' },
    { id: 'categorias', rotulo: 'Categorias', legenda: 'Naturezas e estados' },
    { id: 'contas', rotulo: 'Contas', legenda: 'Rubricas financeiras' },
    { id: 'lancamentos', rotulo: 'Lançamentos', legenda: 'Movimentações e histórico' },
  ];
  protected readonly classificadores: Classificador[] = ['OF', 'OV', 'NOF', 'NOV'];
  protected readonly secaoAtiva = signal<Secao>('visao-geral');
  protected readonly categorias = signal<Categoria[]>([]);
  protected readonly contas = signal<Conta[]>([]);
  protected readonly lancamentos = signal<Lancamento[]>([]);
  protected readonly consolidado = signal<Consolidado | null>(null);
  protected readonly carregando = signal(false);
  protected readonly apiOnline = signal(false);
  protected readonly mensagem = signal<{ texto: string; tom: TomMensagem } | null>(null);

  protected novaCategoria: { nome: string; natureza: Natureza } = {
    nome: '',
    natureza: 'DESPESA',
  };
  protected novaConta: {
    nome: string;
    categoriaId: string;
    classificador: Classificador | '';
  } = { nome: '', categoriaId: '', classificador: 'OF' };
  protected novoLancamento = {
    contaId: '',
    valor: null as number | null,
    dataRealizacao: this.hoje(),
    descricao: '',
  };
  protected filtroContas = { categoriaId: '', ativa: '' };
  protected filtroLancamentos: FiltroLancamentos = {};
  protected periodo = this.periodoInicial();

  protected readonly categoriasAtivas = computed(() =>
    this.categorias().filter((categoria) => categoria.ativa),
  );
  protected readonly contasAtivas = computed(() => this.contas().filter((conta) => conta.ativa));
  protected readonly totalReceitasAtivas = computed(
    () => this.categorias().filter((item) => item.ativa && item.natureza === 'RECEITA').length,
  );
  protected readonly totalDespesasAtivas = computed(
    () => this.categorias().filter((item) => item.ativa && item.natureza === 'DESPESA').length,
  );

  async ngOnInit(): Promise<void> {
    await this.carregarTudo();
  }

  protected selecionarSecao(secao: Secao): void {
    this.secaoAtiva.set(secao);
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

  protected async carregarTudo(): Promise<void> {
    this.carregando.set(true);
    try {
      await Promise.all([
        this.carregarCategorias(false),
        this.carregarContas(false),
        this.carregarLancamentos(false),
        this.carregarConsolidado(false),
      ]);
      this.apiOnline.set(true);
    } catch (erro) {
      this.apiOnline.set(false);
      this.exibirErro(erro);
    } finally {
      this.carregando.set(false);
    }
  }

  protected async criarCategoria(): Promise<void> {
    await this.executar('Categoria criada.', async () => {
      await firstValueFrom(
        this.api.criarCategoria(this.novaCategoria.nome, this.novaCategoria.natureza),
      );
      this.novaCategoria = { nome: '', natureza: 'DESPESA' };
      await this.carregarCategorias(false);
    });
  }

  protected async renomearCategoria(categoria: Categoria): Promise<void> {
    const nome = window.prompt('Novo nome da categoria', categoria.nome)?.trim();
    if (!nome || nome === categoria.nome) return;
    await this.executar('Categoria renomeada.', async () => {
      await firstValueFrom(this.api.renomearCategoria(categoria.id, nome));
      await this.carregarCategorias(false);
    });
  }

  protected async alternarCategoria(categoria: Categoria): Promise<void> {
    await this.executar(categoria.ativa ? 'Categoria inativada.' : 'Categoria reativada.', async () => {
      await firstValueFrom(this.api.alterarEstadoCategoria(categoria.id, !categoria.ativa));
      await this.carregarCategorias(false);
    });
  }

  protected async criarConta(): Promise<void> {
    const categoria = this.categoriaNovaConta();
    if (!categoria) return;
    const classificador = categoria.natureza === 'DESPESA' ? this.novaConta.classificador || null : null;
    await this.executar('Conta criada.', async () => {
      await firstValueFrom(
        this.api.criarConta(this.novaConta.categoriaId, this.novaConta.nome, classificador),
      );
      this.novaConta = { nome: '', categoriaId: '', classificador: 'OF' };
      await this.carregarContas(false);
    });
  }

  protected async renomearConta(conta: Conta): Promise<void> {
    const nome = window.prompt('Novo nome da conta', conta.nome)?.trim();
    if (!nome || nome === conta.nome) return;
    await this.executar('Conta renomeada.', async () => {
      await firstValueFrom(this.api.renomearConta(conta.id, nome));
      await this.carregarContas(false);
    });
  }

  protected async alternarConta(conta: Conta): Promise<void> {
    await this.executar(conta.ativa ? 'Conta inativada.' : 'Conta reativada.', async () => {
      await firstValueFrom(this.api.alterarEstadoConta(conta.id, !conta.ativa));
      await this.carregarContas(false);
    });
  }

  protected async alterarClassificador(conta: Conta, valor: string): Promise<void> {
    if (!valor || valor === conta.classificador) return;
    await this.executar('Classificador atualizado.', async () => {
      await firstValueFrom(this.api.definirClassificador(conta.id, valor as Classificador));
      await this.carregarContas(false);
    });
  }

  protected async classificarLegada(conta: Conta, categoriaId: string): Promise<void> {
    const categoria = this.categorias().find((item) => item.id === categoriaId);
    if (!categoria) return;
    const classificador = categoria.natureza === 'DESPESA' ? 'OF' : null;
    await this.executar('Conta legada classificada.', async () => {
      await firstValueFrom(this.api.classificarContaLegada(conta.id, categoria.id, classificador));
      await this.carregarContas(false);
    });
  }

  protected async registrarLancamento(): Promise<void> {
    if (this.novoLancamento.valor === null) return;
    await this.executar('Lançamento registrado.', async () => {
      await firstValueFrom(
        this.api.registrarLancamento(
          this.novoLancamento.contaId,
          this.novoLancamento.valor!,
          this.novoLancamento.dataRealizacao,
          this.novoLancamento.descricao,
        ),
      );
      this.novoLancamento = {
        contaId: '',
        valor: null,
        dataRealizacao: this.hoje(),
        descricao: '',
      };
      await Promise.all([this.carregarLancamentos(false), this.carregarConsolidado(false)]);
    });
  }

  protected async cancelarLancamento(lancamento: Lancamento): Promise<void> {
    const motivo = window.prompt('Motivo do cancelamento (opcional)', '') ?? '';
    await this.executar('Lançamento cancelado.', async () => {
      await firstValueFrom(this.api.cancelarLancamento(lancamento.id, motivo));
      await Promise.all([this.carregarLancamentos(false), this.carregarConsolidado(false)]);
    });
  }

  protected async carregarCategorias(notificar = true): Promise<void> {
    await this.comCarga(async () => {
      const pagina = await firstValueFrom(this.api.listarCategorias());
      this.categorias.set(pagina.itens);
      if (notificar) this.exibirMensagem('Categorias atualizadas.', 'sucesso');
    });
  }

  protected async carregarContas(notificar = true): Promise<void> {
    await this.comCarga(async () => {
      const pagina = await firstValueFrom(
        this.api.listarContas(this.filtroContas.categoriaId, this.filtroContas.ativa),
      );
      this.contas.set(pagina.itens);
      if (notificar) this.exibirMensagem('Contas atualizadas.', 'sucesso');
    });
  }

  protected async carregarLancamentos(notificar = true): Promise<void> {
    await this.comCarga(async () => {
      const pagina = await firstValueFrom(this.api.listarLancamentos(this.filtroLancamentos));
      this.lancamentos.set(pagina.itens);
      if (notificar) this.exibirMensagem('Lançamentos atualizados.', 'sucesso');
    });
  }

  protected async carregarConsolidado(notificar = true): Promise<void> {
    await this.comCarga(async () => {
      const resumo = await firstValueFrom(
        this.api.obterConsolidado(this.periodo.dataInicial, this.periodo.dataFinal),
      );
      this.consolidado.set(resumo);
      if (notificar) this.exibirMensagem('Consolidado atualizado.', 'sucesso');
    });
  }

  protected nomeCategoria(id: string | null): string {
    if (!id) return 'Não classificada';
    return this.categorias().find((categoria) => categoria.id === id)?.nome ?? 'Categoria removida';
  }

  protected nomeConta(id: string): string {
    return this.contas().find((conta) => conta.id === id)?.nome ?? 'Conta não encontrada';
  }

  protected categoriaNovaConta(): Categoria | undefined {
    return this.categorias().find((categoria) => categoria.id === this.novaConta.categoriaId);
  }

  protected tituloSecao(): string {
    return this.secoes.find((secao) => secao.id === this.secaoAtiva())?.rotulo ?? 'FinTrack';
  }

  protected categoriaDaConta(contaId: string): string | null {
    return this.contas().find((conta) => conta.id === contaId)?.categoriaId ?? null;
  }

  protected naturezaConta(contaId: string): Natureza | null {
    const conta = this.contas().find((item) => item.id === contaId);
    const categoria = this.categorias().find((item) => item.id === conta?.categoriaId);
    return categoria?.natureza ?? null;
  }

  protected fecharMensagem(): void {
    this.mensagem.set(null);
  }

  protected trackById(_: number, item: { id: string }): string {
    return item.id;
  }

  private async executar(mensagem: string, acao: () => Promise<void>): Promise<void> {
    this.carregando.set(true);
    try {
      await acao();
      this.exibirMensagem(mensagem, 'sucesso');
    } catch (erro) {
      this.exibirErro(erro);
    } finally {
      this.carregando.set(false);
    }
  }

  private async comCarga(acao: () => Promise<void>): Promise<void> {
    const jaCarregando = this.carregando();
    if (!jaCarregando) this.carregando.set(true);
    try {
      await acao();
    } catch (erro) {
      if (jaCarregando) throw erro;
      this.exibirErro(erro);
    } finally {
      if (!jaCarregando) this.carregando.set(false);
    }
  }

  private exibirErro(erro: unknown): void {
    if (!(erro instanceof HttpErrorResponse)) {
      this.exibirMensagem('Não foi possível concluir a operação.', 'erro');
      return;
    }
    const problema = (erro.error ?? {}) as ProblemDetails;
    const campos = problema.errors ? Object.values(problema.errors).flat().join(' ') : '';
    const texto = campos || problema.detail || problema.title || `Erro HTTP ${erro.status}.`;
    this.exibirMensagem(texto, 'erro');
  }

  private exibirMensagem(texto: string, tom: TomMensagem): void {
    this.mensagem.set({ texto, tom });
    window.setTimeout(() => {
      if (this.mensagem()?.texto === texto) this.mensagem.set(null);
    }, 5000);
  }

  private hoje(): string {
    return new Date().toISOString().slice(0, 10);
  }

  private periodoInicial(): { dataInicial: string; dataFinal: string } {
    const hoje = new Date();
    const inicio = new Date(hoje.getFullYear(), hoje.getMonth(), 1);
    const fim = new Date(hoje.getFullYear(), hoje.getMonth() + 1, 0);
    const local = (data: Date) => {
      const ano = data.getFullYear();
      const mes = String(data.getMonth() + 1).padStart(2, '0');
      const dia = String(data.getDate()).padStart(2, '0');
      return `${ano}-${mes}-${dia}`;
    };
    return { dataInicial: local(inicio), dataFinal: local(fim) };
  }
}
