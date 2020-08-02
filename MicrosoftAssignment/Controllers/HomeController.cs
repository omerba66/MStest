using System.Net;
using System.Net.Http;
using System.Web.Mvc;

namespace MicrosoftAssignment.Controllers
{

    public class HomeController : Controller
    {
        [HttpGet]
        public HttpResponseMessage Index()
        {
            return new HttpResponseMessage(statusCode: HttpStatusCode.OK);
        }
    }
}
