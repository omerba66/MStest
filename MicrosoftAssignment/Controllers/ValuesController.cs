using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using MicrosoftAssignment.Attributes;

namespace MicrosoftAssignment.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values/5
        [ThrottleFilter]
        public async Task<IHttpActionResult> Get(int id)
        {
            return await Task.FromResult(Ok($"Request from client {id}"));
        }
    }
}
