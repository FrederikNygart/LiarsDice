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

        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [AcceptVerbs("POST")]
        public IHttpActionResult Bid(ObjectId id, [FromBody] Bid bid)
        {
            try
            {
                game.Bid(id, bid);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [AcceptVerbs("POST")]
        public IHttpActionResult BidOfAKind(ObjectId id, [FromBody] Bid bid)
        {
            try
            {
                game.BidOfAKind(id, bid);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [AcceptVerbs("POST")]
        public IHttpActionResult Challenge(ObjectId id)
        {
            try
            {
                game.Challenge(id);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [AcceptVerbs("POST")]
        public IHttpActionResult SpotOn(ObjectId id)
        {
            try
            {
                game.SpotOn(id);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [AcceptVerbs("POST")]
        public IHttpActionResult RollDice(ObjectId id)
        {
            try
            {
                game.RollDice(id);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
