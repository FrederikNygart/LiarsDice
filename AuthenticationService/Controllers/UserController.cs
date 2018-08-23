using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Http.Cors;
using AuthenticationService.DataProviders;
using AuthenticationService.DataTransferObjects;
using MongoDB.Bson;
using Swashbuckle.Swagger.Annotations;

namespace AuthenticationService.Controllers
{
    [RoutePrefix("api/users")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UserController : ApiController
    {

        [SwaggerResponse(HttpStatusCode.OK)]
        [AcceptVerbs("POST")]
        public IHttpActionResult CreateUser([FromBody] User user)
        {
            user.Id = ObjectId.GenerateNewId();
            UserProvider.Insert(user);
            return Ok();
        }

        [SwaggerResponse(HttpStatusCode.OK)]
        [AcceptVerbs("POST")]
        public IHttpActionResult CreateUsers([FromBody] List<User> users)
        {
            users.ForEach(x => x.Id = ObjectId.GenerateNewId());
            UserProvider.InsertMany(users);
            return Ok();
        }

        [SwaggerResponse(HttpStatusCode.OK)]
        [AcceptVerbs("GET")]
        public OkNegotiatedContentResult<List<User>> GetAll()
        {
            return Ok(UserProvider.GetAll());
        }

        [SwaggerResponse(HttpStatusCode.OK)]
        [AcceptVerbs("POST")]
        public OkNegotiatedContentResult<User> GetByPhone([FromBody] User user)
        {
            return Ok(UserProvider.GetBy(user.PhoneNumber));
        }
    }
}

