using BusinessLogic.DataModel;
using BusinessLogic.Services.Seguridad;
using DataAccess.DataBase;
using DemoInternalServices.Models.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace DemoInternalServices.Controllers
{
    public class UsuarioController : ApiController
    {
        [HttpPost]
        [Route("api/Usuario/AddUsuario")]
        public IHttpActionResult AddUsuario([FromBody] UsuarioModel usuario)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                uow.BeginTransaction();

                try
                {
                    SecurityService securityService = new SecurityService();

                    //Genrar SALT
                    string salt = securityService.GenerarSalt(10);

                    //Hashear password usuario
                    string hashedPassword = securityService.GenerarHashSHA256(usuario.Password, salt);

                    //Crear usuario
                    var usuarioEntity = new Usuario()
                    {
                        Password = hashedPassword,
                        UserName = usuario.UserName,
                        PasswordSalt = salt
                    };

                    uow.UsuarioRepository.AddUsuario(usuarioEntity);

                    uow.SaveChanges();
                    uow.Commit();

                    return Ok();
                }
                catch (Exception ex)
                {
                    uow.Rollback();
                    return InternalServerError(ex);
                }
            }
        }
    }
}