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
using System.Web.Http.Cors;

namespace SnydService.Controllers
{
    public  class GameDto{
        public List<string> Players { get; set; }
        public GameOptions GameOptions { get; set; }
    }

    [RoutePrefix("api/game")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
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
        [AcceptVerbs("GET")]
        public IHttpActionResult GetCurrentPlayer(string id)
        {
            try
            {
                return Ok(game.GetCurrentPlayer(ObjectId.Parse(id)));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [AcceptVerbs("GET")]
        public IHttpActionResult GetGame(string id)
        {
            return Ok(game.GetGame(ObjectId.Parse(id)));
        }

        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [AcceptVerbs("POST")]
        public IHttpActionResult Bid(string id, [FromBody] Bid bid)
        {
            try
            {
                game.Bid(ObjectId.Parse(id), bid);
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
        public IHttpActionResult BidOfAKind(string id, [FromBody] Bid bid)
        {
            try
            {
                game.BidOfAKind(ObjectId.Parse(id), bid);
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
        public IHttpActionResult Challenge(string id)
        {
            try
            {
                game.Challenge(ObjectId.Parse(id));
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
        public IHttpActionResult SpotOn(string id)
        {
            try
            {
                game.SpotOn(ObjectId.Parse(id));
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
        public IHttpActionResult RollDice(string id)
        {
            try
            {
                game.RollDice(ObjectId.Parse(id));
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
