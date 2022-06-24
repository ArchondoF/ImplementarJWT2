using DataAccess.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DataModel.Repositories
{
    public class LibroRepository
    {
        //Atributos readonly de una clase en c# comienzan con _
        private readonly PRG3PR0DB _context;

        public LibroRepository(PRG3PR0DB context)
        {
            this._context = context;
        }

        public void AddLibro(Libros libro)
        {
            this._context.Libros.Add(libro);
        }

        public Libros GetLibroById(long id)
        {
            return this._context.Libros.FirstOrDefault(l => l.Id == id);
        }

        public Libros GetLibroByTitle(string titulo)
        {
            return this._context.Libros.FirstOrDefault(l => l.Titulo == titulo);
        }

        public List<Libros> GetLibrosWithAutorOrderBySales()
        {
            return this._context.Libros.Include("Autores").OrderByDescending(l => l.CantidadDeVentas).ToList();
        }

        public void RemoveLibro(Libros libro)
        {
            this._context.Libros.Remove(libro);
        }
    }
}
