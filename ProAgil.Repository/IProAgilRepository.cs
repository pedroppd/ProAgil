using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ProAgil.Domain;

namespace ProAgil.Repository
{
    public interface IProAgilRepository
    {
        void Add<T>(T entity) where T : class;

        void Update<T>(T entity) where T : class;

        void Delete<T>(T entity) where T : class;

        Task<Boolean> SaveChangesAsync();

        Task<List<Evento>> GetAllEventosByTemaAsync(string Tema, bool includePalestrantes);

        Task<List<Evento>> GetAllEventosAsync(bool includePalestrantes);

        Task<Evento> GetEventoById(int Id, bool includePalestrantes);

        Task<Palestrante> GetAllPalestrantesAsyncByName(string name, bool includeEventos);

        Task<Palestrante> GetPalestranteIdAsync(int Id, bool includeEventos);
    }
}
