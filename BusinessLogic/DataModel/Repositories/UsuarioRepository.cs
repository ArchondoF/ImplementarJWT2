using DataAccess.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DataModel.Repositories
{
    public class UsuarioRepository
    {
        private readonly PRG3PR0DB _context;

        public UsuarioRepository(PRG3PR0DB context)
        {
            this._context = context;
        }

        public void AddUsuario(Usuario usuario)
        {
            this._context.Usuario.Add(usuario);
        }

        public Usuario GetUsuarioByUserName(string userName)
        {
            return this._context.Usuario.FirstOrDefault(u => u.UserName == userName);
        }
    }
}
