using MongoDB.Bson;
using SnydService.DataTransferObjects;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SnydService.Controllers
{
    [RoutePrefix("api/game")]
    public class GameController : ApiController
    {

        // POST api/values/5
        [SwaggerOperation("")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [AcceptVerbs("POST")]
        public void StartGame([FromBody]List<ObjectId> users)
        {
            new GameEngine().Start(users);
        }
    }
}
