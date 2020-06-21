using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProAgil.Domain;
using ProAgil.Repository;
using ProAgil.WebAPI.Form;

namespace ProAgil.WebAPI.Controllers
{
    [ApiController]
    [Route("api/eventos")]
    public class EventoController : ControllerBase
    {
        private readonly ProAgilContext _context;
        public ProAgilRepository ProAgilRepository { get; set; }

        public EventoController(ProAgilContext context)
        {
            this._context = context;
            this._context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            this.ProAgilRepository = new ProAgilRepository(_context);
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Get()
        {
            try
            {
                List<Evento> results = await this.ProAgilRepository.GetAllEventosAsync(true);
                return Ok(results);
            }
            catch (Exception e)
            {
                string message = string.Format("Banco de dados falhou {0}", e.Message);
                return this.StatusCode(StatusCodes.Status404NotFound, message);
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int? id)
        {
            try
            {
                Evento result = await this.ProAgilRepository.GetEventoById(id.Value, true);  
                return Ok(result);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status404NotFound, "Erro ao buscar :" + id);
            }
        }

        [HttpPost]
        [Route("saveEvento")]
        public async Task<IActionResult> Insert([FromBody] Evento evento)
        {
            try
            {
                if (evento == null)
                {
                    return BadRequest("Evento Não pode ser nullo");
                }
                
                this.ProAgilRepository.Add(evento);
                bool saved = await this.ProAgilRepository.SaveChangesAsync();

                if (saved)
                {
                    return Created($"/api/eventos/{evento.Id}", evento);
                }

                return BadRequest(string.Format("Ocorreu um erro ao tentar salvar o evento {0}!", evento.Tema));
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status404NotFound, "Erro ao tentar salvar :(");
            }
        }

        [HttpPut]
        [Route("updateEvento/{id}")]
        public async Task<IActionResult> Update(int id,  Evento evento)
        {
            try
            {
                Evento evento1 = await this.ProAgilRepository.GetEventoById(id, false);

                if (evento1 == null)
                {
                    return BadRequest("Evento Não foi encontrado.");
                }

                this.ProAgilRepository.Update(evento);
                bool updated = await this.ProAgilRepository.SaveChangesAsync();

                if (updated)
                {
                    return Created($"/api/eventos/{evento.Id}", evento);
                }

                return BadRequest(string.Format("Ocorreu um erro ao tentar salvar o evento {0}!", evento.Tema));
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status404NotFound, "Erro ao tentar salvar :(");
            }
        }

        [HttpDelete("deleteEvento/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                Evento evento = await this.ProAgilRepository.GetEventoById(id.Value, true);

                if (evento == null)
                {
                    return BadRequest("Evento não existe :(");
                }

                this.ProAgilRepository.Delete(evento);
                bool deleted = await this.ProAgilRepository.SaveChangesAsync();

                if (deleted)
                {
                    return Created($"/api/eventos/{evento.Id}", evento);
                }

                return BadRequest(string.Format("Ocorreu um erro ao tentar salvar o evento {0}!", evento.Tema));
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status404NotFound, "Erro ao tentar salvar :(");
            }
        }
    }
}
