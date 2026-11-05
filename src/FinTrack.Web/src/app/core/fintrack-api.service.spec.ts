import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { FinTrackApiService } from './fintrack-api.service';

describe('FinTrackApiService', () => {
  let api: FinTrackApiService;
  let http: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [FinTrackApiService, provideHttpClient(), provideHttpClientTesting()],
    });
    api = TestBed.inject(FinTrackApiService);
    http = TestBed.inject(HttpTestingController);
  });

  afterEach(() => http.verify());

  it('lists categories with the maximum supported page size', () => {
    api.listarCategorias().subscribe((pagina) => expect(pagina.total).toBe(0));

    const request = http.expectOne(
      (item) => item.url === '/categorias' && item.params.get('tamanhoPagina') === '100',
    );
    expect(request.request.method).toBe('GET');
    request.flush({ itens: [], pagina: 1, tamanhoPagina: 100, total: 0 });
  });

  it('creates a transaction using the public Portuguese contract', () => {
    api.registrarLancamento('conta-1', 125.5, '2026-11-05', 'Mercado').subscribe();

    const request = http.expectOne('/lancamentos');
    expect(request.request.method).toBe('POST');
    expect(request.request.body).toEqual({
      contaId: 'conta-1',
      valor: 125.5,
      dataRealizacao: '2026-11-05',
      descricao: 'Mercado',
    });
    request.flush({});
  });
});
