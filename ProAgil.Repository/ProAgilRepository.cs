using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProAgil.Domain;
using ProAgil.WebAPI;

namespace ProAgil.Repository
{
    public class ProAgilRepository : IProAgilRepository
    {
        private readonly ProAgilContext _context;

        public ProAgilRepository(ProAgilContext context)
        {
            _context = context;
        }

        public ProAgilRepository() {}

        public void Add<T>(T entity) where T : class
        {
            this._context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            this._context.Remove(entity);
        }

        public void Update<T>(T entity) where T : class
        {
            this._context.Update(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {
             return await this._context.SaveChangesAsync() > 0;
        }

        public async Task<List<Evento>> GetAllEventosAsync(bool includePalestrantes = false)
        {
            IQueryable<Evento> query = this._context.Eventos.Include(lotes => lotes).Include(rs => rs.RedesSociais);

            if (includePalestrantes)
            {
                query = query.Include(evento => evento.PalestrantesEventos).ThenInclude(pe => pe.Palestrante);
            }

            query = query.AsNoTracking().OrderByDescending(evento => evento.DataEvento);
            return await query.ToListAsync();
        }

        public async Task<Evento> GetEventoById(int Id, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = this._context.Eventos.Include(lotes => lotes).Include(rs => rs.RedesSociais);

            if (includePalestrantes)
            {
                query = query.Include(evento => evento.PalestrantesEventos).ThenInclude(pe => pe.Palestrante);
            }

            query = query.AsNoTracking().OrderByDescending(evento => evento.DataEvento).Where(evento => evento.Id == Id);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<Evento>> GetAllEventosByTemaAsync(string Tema, bool includePalestrantes)
        {
            IQueryable<Evento> query = this._context.Eventos.Include(lotes => lotes).Include(rs => rs.RedesSociais);

            if (includePalestrantes)
            {
                query = query.Include(evento => evento.PalestrantesEventos).ThenInclude(pe => pe.Palestrante);
            }

            query = query.AsNoTracking().OrderByDescending(evento => evento.DataEvento).Where(evento => evento.Tema.ToUpper().Equals(Tema.ToUpper())); 
            return await query.ToListAsync();
        }

        public async Task<Palestrante> GetAllPalestrantesAsyncByName(string name, bool includeEventos)
        {
            IQueryable<Palestrante> query = this._context.Palestrantes.Include(lotes => lotes).Include(rs => rs.RedesSociais);

            if (includeEventos)
            {
                query = query.Include(palestrante => palestrante.PalestrantesEventos).ThenInclude(pe => pe.Evento);
            }

            query = query.AsNoTracking().OrderByDescending(palestrante => palestrante.Nome).Where(palestrante => palestrante.Nome.ToUpper().Equals(name.ToUpper()));
            return await query.FirstOrDefaultAsync();
        }

        public async Task<Palestrante> GetPalestranteIdAsync(int Id, bool includeEventos)
        {
            IQueryable<Palestrante> query = this._context.Palestrantes.Include(lotes => lotes).Include(rs => rs.RedesSociais);

            if (includeEventos)
            {
                query = query.Include(palestrante => palestrante.PalestrantesEventos).ThenInclude(pe => pe.Evento);
            }

            query = query.AsNoTracking().OrderByDescending(palestrante => palestrante.Nome).Where(palestrante => palestrante.Id == Id);

            return await query.FirstOrDefaultAsync();
        }
    }
}
