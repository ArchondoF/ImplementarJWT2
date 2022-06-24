using BusinessLogic.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Services.Controllers
{
    public class AutorController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetAutor(int id)
        {
            
            using (var uow = new UnitOfWork())
            {
                var autor = uow.AutorRepository.GetAutorById(id);

                return Ok(autor);
            }

        }
    }
}