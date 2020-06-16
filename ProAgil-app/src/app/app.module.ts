
/*COMPONENT*/
import { AppComponent } from './app.component';
import { NavComponent } from './nav/nav.component';
/*MODULES*/
import { EventosComponent } from './eventos/eventos.component';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { ModalModule } from 'ngx-bootstrap/modal';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
/*DATE PIPE*/
import { DatePipe } from '@angular/common';
import { DateTimePipe } from './_helps/DateTime.pipe';
/*SERVICES*/
import { EventoService } from 'src/_services/Evento.service';

@NgModule({
   declarations: [
      AppComponent,
      EventosComponent,
      NavComponent,
      DateTimePipe
   ],
   imports: [
      BrowserModule,
      BsDropdownModule.forRoot(),
      TooltipModule.forRoot(),
      ModalModule.forRoot(),
      HttpClientModule,
      FormsModule,
      BrowserAnimationsModule
   ],
   providers: [
      EventoService
   ],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
