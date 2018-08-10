using MongoDB.Bson;
using SnydService.DataTransferObjects;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebUtil;

namespace SnydService.Controllers
{
    [RoutePrefix("api/game")]
    public class GameController : ApiController
    {
        private GameEngine game = new GameEngine();

        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [SwaggerResponse(HttpStatusCode.PreconditionFailed)]
        [AcceptVerbs("POST")]
        public UpdateResponse Start([FromBody]List<string> usersIds)
        {
            try
            {
                var users = usersIds.Select(userId => ObjectId.Parse(userId));
                game.Start(users);
                return new UpdateResponse { Status = HttpStatusCode.OK };
            }
            catch (NullReferenceException e)
            {
                return new UpdateResponse {
                    Status = HttpStatusCode.PreconditionFailed,
                    UserMessage = "Have you set the Game Options?",
                    Exception = e.Message
                };
            }
        }

        [SwaggerResponse(HttpStatusCode.OK)]
        [Route("~/api/setgameoptions")]
        [AcceptVerbs("POST")]
        public UpdateResponse SetGameOptions([FromBody] GameOptions gameOptions)
        {
            game.SetGameOptions(gameOptions);
            return new UpdateResponse { Status = HttpStatusCode.OK };
        }
    }
}
