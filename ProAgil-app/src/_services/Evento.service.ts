import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Evento } from 'src/_models/Evento';

@Injectable({
  providedIn: 'root'
})
export class EventoService {

  constructor(private http: HttpClient) { }

BaseUrl = 'https://localhost:44303/api/eventos/';

  GetAllEventos(): Observable<Evento[]>
  {
    return this.http.get<Evento[]>(this.BaseUrl);
  }

  GetEventoById(Id: number): Observable<Evento>
  {
    return this.http.get<Evento>(`${this.BaseUrl}/${Id}`);
  }

  GetEventoByTema(Tema: string): Observable<Evento[]>
  {
    return this.http.get<Evento[]>(`${this.BaseUrl}/GetByTema/${Tema}`);
  }
}
