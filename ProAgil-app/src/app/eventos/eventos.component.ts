import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.css']
})
export class EventosComponent implements OnInit {

  constructor(private httpClient: HttpClient) { }

   eventos: any = [];

  ngOnInit(){
    this.GetEventos();
  }

  GetEventos()
  {
     return this.httpClient.get('http://localhost:5000/value').subscribe(
      response => {
        console.log(response);
        this.eventos = response;
      }, error => {
        console.log(error);
      }
    );
  }

}
