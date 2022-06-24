using BusinessLogic.DataModel;
using DataAccess.DataBase;
using DemoInternalServices.Models.Autor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace DemoInternalServices.Controllers
{
    //[Authorize]
    public class AutorController : ApiController
    {
        /// <summary>
        /// GetAutorById - Consulta un autor dado un identificador
        /// </summary>
        /// <param name="id">Identificador del autor - long - Requerido</param>
        /// <returns>
        /// HttpCode 200: Si el autor existe retorna el mismo
        /// HttpCode 404: El autor no existe para el id dado
        /// HttpCode 500: Error no controlado
        /// </returns>
        [HttpGet]
        [Route("api/Autor/GetAutorById")]
        public IHttpActionResult GetAutorById(long id)
        {
            try
            {
                using (var uow = new UnitOfWork())
                {
                    var autor = uow.AutorRepository.GetAutorById(id);

                    if (autor == null)
                        return NotFound();

                    return Ok(autor);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Verifica si existe un autor para un nombre dado, retorna un valor booleano representando si existe o no
        /// </summary>
        /// <param name="name">Nombre autor</param>
        /// <returns>True si existe, false si no existe</returns>
        [HttpGet]
        [Route("api/Autor/ExistAutorByName")]
        public IHttpActionResult ExistAutorByName(string name)
        {
            try
            {
                using (var uow = new UnitOfWork())
                {
                    var existe = uow.AutorRepository.AnyAutorByName(name);

                    return Ok(existe);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/Autor/GetAutores")]
        public IHttpActionResult GetAutores()
        {
            try
            {
                using (var uow = new UnitOfWork())
                {
                    var autores = uow.AutorRepository.GetAutores();

                    if (autores == null)
                        return NotFound();

                    return Ok(autores);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// AddAutor - Crea un nuevo Autor
        /// </summary>
        /// <param name="autor">Objeto autor del tipo AutorModel - Requerido</param>
        /// <returns>
        /// HttpCode 200: El autor es creado correctamente, el mismo es retornado
        /// HttpCode 500: Error no controlado
        /// </returns>
        [HttpPost]
        [Route("api/Autor/AddAutor")]
        public IHttpActionResult AddAutor([FromBody] AutorModel autor)
        {
            using (var uow = new UnitOfWork())
            {
                uow.BeginTransaction();

                try
                {
                    var autorEntity = new Autores()
                    {
                        Nombre = autor.Nombre
                    };

                    uow.AutorRepository.AddAutor(autorEntity);

                    uow.SaveChanges();
                    uow.Commit();

                    return Ok(autorEntity);
                }
                catch (Exception ex)
                {
                    uow.Rollback();
                    return InternalServerError(ex);
                }
            }
        }

        /// <summary>
        /// UpdateAutor - Actualiza un autor en su totalidad
        /// </summary>
        /// <param name="autor">Objeto autor del tipo AutorModel - Requerido</param>
        /// <returns>
        /// HttpCode 200: El autor es actualizado de manera exitosa, el mismo es retornado
        /// HttpCode 404: El autor a modificar no existe
        /// HttpCode 500: Error no controlado
        /// </returns>
        [HttpPut]
        [Route("api/Autor/UpdateAutor")]
        public IHttpActionResult UpdateAutor([FromBody] AutorModel autor)
        {
            using (var uow = new UnitOfWork())
            {
                uow.BeginTransaction();

                try
                {
                    var autorEntity = uow.AutorRepository.GetAutorById(autor.Id);

                    if (autorEntity == null)
                        return NotFound();

                    autorEntity.Nombre = autor.Nombre;

                    uow.SaveChanges();
                    uow.Commit();

                    return Ok(autorEntity);
                }
                catch (Exception ex)
                {
                    uow.Rollback();
                    return InternalServerError(ex);
                }
            }
        }

        /// <summary>
        /// UpdateNombreAutor - Actualiza el nombre de un autor
        /// </summary>
        /// <param name="id">Identificador del autor - long - Requerido</param>
        /// <param name="nombre">Nombre del autor - string - Requerido, largo maximo 50</param>
        /// <returns>
        /// HttpCode 200: El nombre del autor es actualizado de manera exitosa, el mismo es retornado
        /// HttpCode 404: El autor a modificar no existe
        /// HttpCode 400: Error en los datos recibidos
        /// HttpCode 500: Error no controlado
        /// </returns>
        [HttpPatch]
        [Route("api/Autor/UpdateNombreAutor")]
        public IHttpActionResult UpdateNombreAutor(long id, string nombre)
        {
            using (var uow = new UnitOfWork())
            {
                uow.BeginTransaction();

                try
                {
                    var autorEntity = uow.AutorRepository.GetAutorById(id);

                    if (autorEntity == null)
                        return NotFound();

                    if (string.IsNullOrEmpty(nombre))
                        return BadRequest("Nombre requerido");

                    autorEntity.Nombre = nombre;

                    uow.SaveChanges();
                    uow.Commit();

                    return Ok(autorEntity);
                }
                catch (Exception ex)
                {
                    uow.Rollback();
                    return InternalServerError(ex);
                }
            }
        }

        /// <summary>
        /// RemoveAutor - Elimina un autor
        /// </summary>
        /// <param name="id">Identificador del autor - long - Requerido</param>
        /// <returns>
        /// HttpCode 200: El autor fue removido de manera exitosa
        /// HttpCode 404: El autor a remover no existe
        /// HttpCode 500: Error no controlado
        /// </returns>
        [HttpDelete]
        [Route("api/Autor/RemoveAutor")]
        public IHttpActionResult RemoveAutor(long id)
        {
            using (var uow = new UnitOfWork())
            {
                uow.BeginTransaction();

                try
                {
                    var autorEntity = uow.AutorRepository.GetAutorById(id);

                    if (autorEntity == null)
                        return NotFound();

                    uow.AutorRepository.RemoveAutor(autorEntity);

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