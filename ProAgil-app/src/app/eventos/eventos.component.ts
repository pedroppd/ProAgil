import { Component, OnInit, TemplateRef } from '@angular/core';
import { EventoService } from 'src/_services/Evento.service';
import { Evento } from 'src/_models/Evento';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import {BsLocaleService} from 'ngx-bootstrap/datepicker';
import { defineLocale } from 'ngx-bootstrap/chronos';
import { ptBrLocale } from 'ngx-bootstrap/locale';
defineLocale('pt-br', ptBrLocale);

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.css']
})
export class EventosComponent implements OnInit {

  constructor(private eventoService: EventoService, private ModalService: BsModalService,
              private Fb: FormBuilder, private LocaleService: BsLocaleService){
                this.LocaleService.use('pt-br');
              }

  eventosList: Evento[];
  // tslint:disable-next-line:variable-name
  _filterList: string;
  ShowImage = false;
  EventFilters: Evento[];
  Evento: Evento;
  RegisterForm: FormGroup;
  SaveOrUpdateOrDelete: string;
  DELETE = 'Delete';
  SAVE = 'Save';
  UPDATE = 'Update';

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
    this.Validation();
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

  OpenModal(Template: any)
  {
    this.RegisterForm.reset();
    Template.show();
  }

  SalvarAlteracao(Template: any)
  {
    if (this.SaveOrUpdateOrDelete === this.UPDATE && this.RegisterForm.valid)
    {
      this.Evento = Object.assign({id: this.Evento.id}, this.RegisterForm.value);
      this.eventoService.UpdateEvento(this.Evento).subscribe((response: any) => {
      Template.hide();
      this.GetEventos();
      }, Error => {
        console.log(Error);
      });
    }
    if (this.SaveOrUpdateOrDelete === this.SAVE && this.RegisterForm.valid)
    {
      this.Evento = Object.assign({}, this.RegisterForm.value);
      this.eventoService.SaveEvento(this.Evento).subscribe((response: any) => {
      console.log(response);
      Template.hide();
      this.GetEventos();
    }, Error => {
      console.log(Error);
    });
    }
    if (this.SaveOrUpdateOrDelete === this.DELETE)
    {
      this.Evento = Object.assign({id: this.Evento.id}, this.RegisterForm.value);
      this.eventoService.DeleteEvento(this.Evento).subscribe((response: any) => {
      console.log(response);
      Template.hide();
      this.GetEventos();
    }, Error => {
      console.log(Error);
    });
    }
  }

  Validation()
  {
    this.RegisterForm = this.Fb.group({
      tema: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
      local: ['', Validators.required],
      dataEvento: ['', Validators.required],
      qtdPessoas: ['', [Validators.required, Validators.max(120000)]],
      imagemURL: ['', Validators.required],
      telefone: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]]});
  }

  UpdateEventoModal(Template: any, evento: Evento)
  {
    this.SaveOrUpdateOrDelete = 'Update';
    this.OpenModal(Template);
    this.Evento = evento;
    this.RegisterForm.patchValue(this.Evento);
  }

  SaveEventoModal(Template: any)
  {
    this.SaveOrUpdateOrDelete = 'Save';
    this.OpenModal(Template);
  }

  DeleteEventoModal(Template: any,  evento: Evento)
  {
    this.SaveOrUpdateOrDelete = 'Delete';
    this.OpenModal(Template);
    this.Evento = evento;
    this.RegisterForm.patchValue(this.Evento);
  }
}
