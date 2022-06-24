using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace DemoInternalServices.Seguridad
{
    /// <summary>
    /// Hndler HTTP para validacion de token de autenticacion JWT
    /// </summary>
    public class ManejadorDeValidacionDeTokens : DelegatingHandler
    {
        /// <summary>
        /// Oberride metodo SendAsync para interceptar llamadas HTTP y validar el token JWT
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpStatusCode codigoEstado;
            string token = string.Empty;

            //Intenatar obtener token JWT del header de la llamda HTTP
            if (!IntentarObtenerToken(request, out token))
            {
                return base.SendAsync(request, cancellationToken);
            }

            try
            {
                //Obtener parametros de generacion de token JWT para su validacion
                var claveSecreta = ConfigurationManager.AppSettings["JWT_SECRET_KEY"];
                var audiencia = ConfigurationManager.AppSettings["JWT_AUDIENCE_TOKEN"];
                var emisor = ConfigurationManager.AppSettings["JWT_ISSUER_TOKEN"];

                //Generar clave de seguridad para validar la firma digital
                var claveDeSeguridad = new SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(claveSecreta));

                SecurityToken securityToken;
                var tokenHandler = new JwtSecurityTokenHandler();

                //Validacion de token JWT
                TokenValidationParameters validationParameters = new TokenValidationParameters()
                {
                    ValidAudience = audiencia,
                    ValidIssuer = emisor,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    LifetimeValidator = this.ValidarTiempoDeVidaToken,
                    IssuerSigningKey = claveDeSeguridad
                };

                //Extraer y asignar el principal de identidad al hilo, idem para el usuario del contexto http
                var tokenValido = tokenHandler.ValidateToken(token, validationParameters, out securityToken);

                Thread.CurrentPrincipal = tokenValido;
                HttpContext.Current.User = tokenValido;

                return base.SendAsync(request, cancellationToken);
            }
            catch (SecurityTokenValidationException tex)
            {
                codigoEstado = HttpStatusCode.Unauthorized;
            }
            catch (ArgumentException aex)
            {
                codigoEstado = HttpStatusCode.Unauthorized;
            }

            catch (Exception ex)
            {

                codigoEstado = HttpStatusCode.InternalServerError;
            }

            return Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(codigoEstado) { });
        }

        /// <summary>
        /// Obtiene el token JWT del header de la solicitud HTTP, so no lo puede obtener retorna false
        /// </summary>
        /// <param name="request">Request HTTP</param>
        /// <param name="token">Parametro de salida para token JWT</param>
        /// <returns></returns>
        public static bool IntentarObtenerToken(HttpRequestMessage request, out string token)
        {
            token = null;
            bool tokenObtenido = false;
            IEnumerable<string> authHeaders;

            if (request.Headers.TryGetValues("Authorization", out authHeaders) && authHeaders.Count() == 1)
            {
                var bearerToken = authHeaders.ElementAt(0);

                token = bearerToken.StartsWith("Bearer ") ? bearerToken.Substring(7) : bearerToken;

                tokenObtenido = true;
            }

            return tokenObtenido;
        }

        /// <summary>
        /// Evalua si el tiempo de vida del token aun esta vigente
        /// </summary>
        /// <param name="notBefore">Fecha de comienzo de uso del token</param>
        /// <param name="expires">Fecha de vencimiento del token</param>
        /// <param name="securityToken">Security token</param>
        /// <param name="validationParameters">Parametros de validacion token jwt</param>
        /// <returns>Booleano, si token es valido true, false si el token expiro</returns>
        public bool ValidarTiempoDeVidaToken(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            bool tokenVivo = false;

            if (expires != null && DateTime.UtcNow < expires)
            {
                tokenVivo = true;
            }

            return tokenVivo;
        }
    }
}