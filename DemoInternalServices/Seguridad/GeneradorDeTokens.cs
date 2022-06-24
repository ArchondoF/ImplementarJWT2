using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace DemoInternalServices.Seguridad
{
    /// <summary>
    /// Clase encargada de la generacion de tokens JWT para la autenticacion de usuarios en la WEB API
    /// </summary>
    public class GeneradorDeTokens
    {
        /// <summary>
        /// Generador de token JWT para la identidad de usuario
        /// </summary>
        /// <param name="ussername">Nombre de usuario unico - string</param>
        /// <returns>Token JWT</returns>
        public static string GenerarToken(string username)
        {
            string token = string.Empty;

            //Obtenemos configuraciones de comportamiento y generacion del token JWT
            var claveSecreta = ConfigurationManager.AppSettings["JWT_SECRET_KEY"];
            var audiencia = ConfigurationManager.AppSettings["JWT_AUDIENCE_TOKEN"];
            var emisor = ConfigurationManager.AppSettings["JWT_ISSUER_TOKEN"];
            var tiempoDeVida = ConfigurationManager.AppSettings["JWT_EXPIRE_MINUTES"];

            //Generamos calve de seguridad a ser utilizada posteriormente en la generacion de credenciales de firma digital
            var claveDeSeguridad = new SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(claveSecreta));

            //Crear credenciales para firma digital
            var credencialesDeFirmaDigital = new SigningCredentials(claveDeSeguridad, SecurityAlgorithms.HmacSha256Signature);

            //Crear claims/afirmaciones de identidad
            ClaimsIdentity claimsDeIdentidad = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) });

            //Crear token JWT
            var tokenHandler = new JwtSecurityTokenHandler();

            var jwtSecurityToken = tokenHandler.CreateJwtSecurityToken(
                audience: audiencia,
                issuer: emisor,
                subject: claimsDeIdentidad,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(tiempoDeVida)),
                signingCredentials: credencialesDeFirmaDigital
                );

            token = tokenHandler.WriteToken(jwtSecurityToken);

            return token;
        }
    }
}