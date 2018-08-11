using MongoDB.Bson;
using Newtonsoft.Json.Linq;
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
    public  class GameDto{
        public List<string> Players { get; set; }
        public GameOptions GameOptions { get; set; }
    }

    [RoutePrefix("api/game")]
    public class GameController : ApiController
    {
        private GameEngine game = new GameEngine();

        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [SwaggerResponse(HttpStatusCode.PreconditionFailed)]
        [AcceptVerbs("POST")]
        public IHttpActionResult Start([FromBody]GameDto g)
        {
            try
            {
                var userIds = g.Players.Select(userId => ObjectId.Parse(userId));

                if (userIds.Count() < 1) return BadRequest();

                return Ok(game.Start(userIds, g.GameOptions));
            }
            catch (NullReferenceException e)
            {
                return BadRequest(e.Message);
            }
        }
        
        public IHttpActionResult Bid(ObjectId id, [FromBody] Bid bid)
        {
            game.Bid(id, bid.Quantity, bid.Quantity);
            return Ok();
        }
    }
}
