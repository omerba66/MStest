using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using MicrosoftAssignment.Attributes;

namespace MicrosoftAssignment.Controllers
{
    public class ValuesController : ApiController
    {
        [ThrottleFilter]
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
    }
}
