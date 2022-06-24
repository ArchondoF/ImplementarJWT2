using DataAccess.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DataModel.Repositories
{
    public class AutorRepository
    {
        //Atributos readonly de una clase en c# comienzan con _
        private readonly PRG3PR0DB _context;

        public AutorRepository(PRG3PR0DB context)
        {
            this._context = context;
        }

        public void AddAutor(Autores libro)
        {
            this._context.Autores.Add(libro);
        }

        public Autores GetAutorById(long id)
        {
            return this._context.Autores.FirstOrDefault(l => l.Id == id);
        }

        public Autores GetAutorByName(string nombre)
        {
            return this._context.Autores.FirstOrDefault(l => l.Nombre == nombre);
        }

        public List<Autores> GetAutores()
        {
            return this._context.Autores.ToList();
        }

        public bool AnyAutorByName(string nombre)
        {
            return this._context.Autores.Any(a => a.Nombre == nombre);
        }

        public List<Autores> GetAutoresOrderByName()
        {
            return this._context.Autores.OrderBy(a => a.Nombre).ToList();
        }

        public void RemoveAutor(Autores libro)
        {
            this._context.Autores.Remove(libro);
        }

    }
}
