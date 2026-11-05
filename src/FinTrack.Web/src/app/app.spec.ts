import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { App } from './app';

describe('App', () => {
  beforeAll(() => {
    Object.defineProperty(window, 'scrollTo', { value: () => undefined, writable: true });
  });

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [App],
      providers: [provideHttpClient(), provideHttpClientTesting()],
    }).compileComponents();
  });

  it('should create the app', () => {
    const fixture = TestBed.createComponent(App);
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();
  });

  it('should render the financial dashboard', async () => {
    const fixture = TestBed.createComponent(App);
    fixture.detectChanges();
    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelector('h1')?.textContent).toContain('Visão geral');
    expect(compiled.textContent).toContain('Seu dinheiro, sem ruído.');
  });

  it('shows the classifier selector after selecting an expense category', async () => {
    const fixture = TestBed.createComponent(App);
    fixture.detectChanges();
    const http = TestBed.inject(HttpTestingController);

    http.expectOne((request) => request.url === '/categorias').flush({
      itens: [{ id: 'categoria-1', nome: 'Moradia', natureza: 'DESPESA', ativa: true }],
      pagina: 1,
      tamanhoPagina: 100,
      total: 1,
    });
    http.expectOne((request) => request.url === '/contas').flush({
      itens: [{
        id: 'conta-1',
        categoriaId: 'categoria-1',
        nome: 'Aluguel',
        classificador: 'OF',
        ativa: true,
      }],
      pagina: 1,
      tamanhoPagina: 100,
      total: 1,
    });
    http.expectOne((request) => request.url === '/lancamentos').flush({
      itens: [], pagina: 1, tamanhoPagina: 100, total: 0,
    });
    http.expectOne((request) => request.url === '/consolidados').flush({
      receitas: 0,
      despesas: 0,
      resultado: 0,
      dataInicial: '2026-11-01',
      dataFinal: '2026-11-30',
      porConta: [],
      porCategoria: [],
      porClassificador: [],
    });
    await fixture.whenStable();

    const app = fixture.componentInstance as unknown as {
      selecionarSecao(secao: string): void;
      novaConta: { categoriaId: string };
    };
    app.selecionarSecao('contas');
    app.novaConta.categoriaId = 'categoria-1';
    fixture.detectChanges();

    const compiled = fixture.nativeElement as HTMLElement;
    const selector = compiled.querySelector('select[name="classificadorConta"]');
    expect(selector).not.toBeNull();
    expect(selector?.querySelectorAll('option')).toHaveLength(4);
    expect(compiled.querySelector('tbody .pill')?.textContent?.trim()).toBe('OF');
    expect(compiled.querySelector('tbody select.compact-select')).toBeNull();
  });
});
