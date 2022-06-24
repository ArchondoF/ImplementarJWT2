using BusinessLogic.Services.Seguridad;
using DemoInternalServices.Models.Login;
using DemoInternalServices.Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace DemoInternalServices.Controllers
{
    [AllowAnonymous]
    public class LoginController : ApiController
    {
        [HttpPost]
        [Route("api/Login/Autenticar")]
        public IHttpActionResult Autenticar([FromBody] LoginModel login)
        {
            try
            {
                SecurityService securityService = new SecurityService();

                if (securityService.ValidarCredencialesUsuario(login.Username, login.Password))
                {
                    //Generacion de token JWT
                    var token = GeneradorDeTokens.GenerarToken(login.Username);

                    return Ok(token);
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}