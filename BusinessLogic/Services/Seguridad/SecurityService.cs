using BusinessLogic.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services.Seguridad
{
    public class SecurityService
    {
        public bool ValidarCredencialesUsuario(string userName, string password)
        {
            bool credencialesValidas = false;

            using (UnitOfWork uow = new UnitOfWork())
            {
                //Obtener usuario
                var usuario = uow.UsuarioRepository.GetUsuarioByUserName(userName);

                if (usuario != null)
                {
                    //Hashear password
                    var hashedPassword = GenerarHashSHA256(password, usuario.PasswordSalt);

                    //Comparo passwords hasheadas con el mismo salt, si coinciden, se que las credenciales son validas
                    if (usuario.Password == hashedPassword)
                        credencialesValidas = true;
                }
            }

            return credencialesValidas;
        }

        public string GenerarHashSHA256(string plainString, string salt)
        {
            StringBuilder stringBuilder = new StringBuilder();

            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding encoder = Encoding.UTF8;

                Byte[] bytes = hash.ComputeHash(encoder.GetBytes(plainString + salt));

                foreach (Byte bite in bytes)
                {
                    stringBuilder.Append(bite.ToString("x2"));
                }
            }

            return stringBuilder.ToString();
        }

        public string GenerarSalt(int tamanoBuffer)
        {
            var rng = new RNGCryptoServiceProvider();

            var buffer = new byte[tamanoBuffer];

            rng.GetBytes(buffer);

            return Convert.ToBase64String(buffer);
        }
    }
}
