using BusinessLogic.DataModel.Repositories;
using DataAccess.DataBase;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DataModel
{
    public class UnitOfWork : IDisposable
    {
        protected readonly PRG3PR0DB _context;
        protected DbContextTransaction Transaction;

        public AutorRepository AutorRepository { get; set; }
        public LibroRepository LibroRepository { get; set; }
        public UsuarioRepository UsuarioRepository { get; set; }

        public UnitOfWork()
        {
            this._context = new PRG3PR0DB();

            this.AutorRepository = new AutorRepository(this._context);
            this.LibroRepository = new LibroRepository(this._context);
            this.UsuarioRepository = new UsuarioRepository(this._context);
        }

        public void SaveChanges()
        {
            this._context.SaveChanges();
        }

        public void BeginTransaction()
        {
            this.Transaction = this._context.Database.BeginTransaction();
        }

        public void Commit()
        {
            if (this.Transaction != null)
                this.Transaction.Commit();
        }

        public void Rollback()
        {
            if (this.Transaction != null)
                this.Transaction.Rollback();
        }

        public void Dispose()
        {
            if (this.Transaction != null)
                this.Transaction.Dispose();

            this._context.Dispose();
        }
    }
}
