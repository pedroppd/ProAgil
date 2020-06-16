using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProAgil.Domain;
using ProAgil.Repository;

namespace ProAgil.WebAPI.Controllers
{
    [ApiController]
    [Route("api/palestrantes/")]
    public class PalestranteController : ControllerBase
    {
        private readonly ProAgilContext _context;
        public ProAgilRepository ProAgilRepository { get; set; }

        public PalestranteController(ProAgilContext context)
        {
            this._context = context;
            this.ProAgilRepository = new ProAgilRepository(_context);
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> Get(string name)
        {
            try
            {
                Palestrante results = await this.ProAgilRepository.GetAllPalestrantesAsyncByName(name, true);
                return Ok(results);
            }
            catch (Exception e)
            {
                string message = string.Format("Banco de dados falhou {0}", e.Message);
                return this.StatusCode(StatusCodes.Status404NotFound, message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int? id)
        {
            try
            {
                Palestrante result = await this.ProAgilRepository.GetPalestranteIdAsync(id.Value, true);
                return Ok(result);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status404NotFound, "Erro ao buscar :" + id);
            }
        }

        [HttpPost]
        [Route("savePalestrante")]
        public async Task<IActionResult> Insert([FromBody] Palestrante palestrante)
        {
            try
            {
                if (palestrante == null)
                {
                    return BadRequest("Palestrante Não pode ser nullo");
                }

                this.ProAgilRepository.Add(palestrante);
                bool saved = await this.ProAgilRepository.SaveChangesAsync();

                if (saved)
                {
                    return Created($"/api/palestrantes/{palestrante.Id}", palestrante);
                }

                return BadRequest(string.Format("Ocorreu um erro ao tentar salvar o palestrante {0}!", palestrante.Nome));
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status404NotFound, "Erro ao tentar salvar :(");
            }
        }

        [HttpPut]
        [Route("updatePalestrante")]
        public async Task<IActionResult> Update(int id, Palestrante palestrante)
        {
            try
            {
                Palestrante palestrante1 = await this.ProAgilRepository.GetPalestranteIdAsync(id, false);

                if (palestrante1 == null)
                {
                    return BadRequest("Palestrante Não encontrado");
                }

                this.ProAgilRepository.Update(palestrante);
                bool saved = await this.ProAgilRepository.SaveChangesAsync();

                if (saved)
                {
                    return Created($"/api/palestrantes/{palestrante.Id}", palestrante);
                }

                return BadRequest(string.Format("Ocorreu um erro ao tentar salvar o palestrante {0}!", palestrante.Nome));
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status404NotFound, "Erro ao tentar salvar :(");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
              
                Palestrante palestrante = await this.ProAgilRepository.GetPalestranteIdAsync(id.Value, true);

                if (palestrante == null)
                {
                    return BadRequest("palestrante não existe :(");
                }
                this.ProAgilRepository.Delete(palestrante);
                bool deleted = await this.ProAgilRepository.SaveChangesAsync();

                if (deleted)
                {
                    return Created($"/api/palestrantes/{palestrante.Id}", palestrante);
                }

                return BadRequest(string.Format("Ocorreu um erro ao tentar salvar o evento {0}!", palestrante.Nome));
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status404NotFound, "Erro ao tentar salvar :(");
            }
        }
    }
}
