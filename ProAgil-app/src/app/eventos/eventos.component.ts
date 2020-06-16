import { Component, OnInit, TemplateRef } from '@angular/core';
import { EventoService } from 'src/_services/Evento.service';
import { Evento } from 'src/_models/Evento';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.css']
})
export class EventosComponent implements OnInit {

  constructor(private eventoService: EventoService, private ModalService: BsModalService) { }

  eventosList: Evento[];
  // tslint:disable-next-line:variable-name
  _filterList: string;
  ShowImage = false;
  EventFilters: Evento[];
  modalRef: BsModalRef;

  get filterList(): string /*Getting of variable filterList*/
  {
    return this._filterList;
  }

  set filterList(obj: string) /*Setting of variable filterList*/
  {
    this._filterList = obj;
    this.EventFilters = this.filterList ? this.FiltrarEventos(this.filterList) : this.eventosList;
  }

  ngOnInit(){
    this.GetEventos();
  }

  GetEventos()
  {
     return this.eventoService.GetAllEventos().subscribe(
      (eventos: Evento[]) => {
        console.log(eventos);
        this.eventosList = eventos;
        this.EventFilters = this.eventosList;
      }, error => {
        console.log(error);
      }
    );
  }

  AlterImage()
  {
    this.ShowImage = !this.ShowImage;
  }

  FiltrarEventos(FilterBy: string): Evento[]
  {
    FilterBy = FilterBy.toLocaleLowerCase();
    return this.eventosList.filter(
      evento => evento.tema.toLocaleLowerCase().indexOf(FilterBy) !== -1
    );
  }

  OpenModal(Template: TemplateRef<any>)
  {
    this.modalRef = this.ModalService.show(Template);
  }

}
