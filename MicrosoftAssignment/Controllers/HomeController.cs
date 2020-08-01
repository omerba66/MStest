using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using MicrosoftAssignment.Attributes;

namespace MicrosoftAssignment.Controllers
{

    public class HomeController : Controller
    {
        [ThrottleFilter]
        [HttpGet]
        [Route("/{clientId:int}")]
        public async Task<HttpResponseMessage> Index(int clientId = 3)
        {
            return new HttpResponseMessage(statusCode: HttpStatusCode.OK);
        }
    }
}
