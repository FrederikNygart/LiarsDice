using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AuthenticationService.DataProviders;
using AuthenticationService.DataTransferObjects;
using MongoDB.Bson;
using Swashbuckle.Swagger.Annotations;

namespace AuthenticationService.Controllers
{
    [RoutePrefix("api/users")]
    public class UserController : ApiController
    {

        [SwaggerResponse(HttpStatusCode.OK)]
        [Route("~/api/createuser")]
        [AcceptVerbs("POST")]
        public void CreateUser([FromBody] User user)
        {
            user.Id = ObjectId.GenerateNewId();
            UserProvider.Insert(user);
        }

        [SwaggerResponse(HttpStatusCode.OK)]
        [Route("~/api/createusers")]
        [AcceptVerbs("POST")]
        public void CreateUsers([FromBody] List<User> users)
        {
            users.ForEach(x => x.Id = ObjectId.GenerateNewId());
            UserProvider.InsertMany(users);
        }
    }
}

