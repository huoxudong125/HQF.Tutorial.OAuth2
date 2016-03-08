using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HQF.Tutorial.OAuth2.ResourceServer.Models;

namespace HQF.Tutorial.OAuth2.ResourceServer.Controllers
{
    [RoutePrefix("api/Orders")]
    public class OrdersController : ApiController
    {
        [Authorize]
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(Order.CreateOrders());
        }

    }

    #region Helpers

    #endregion
}
