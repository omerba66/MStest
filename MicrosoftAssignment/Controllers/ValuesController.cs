﻿using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using MicrosoftAssignment.Attributes;

namespace MicrosoftAssignment.Controllers
{
    public class ValuesController : ApiController
    {

        // GET api/values
        [ThrottleFilter]
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }




        // GET api/values/5
        [ThrottleFilter]
        public async Task<IHttpActionResult> Get(int id)
        {
            return await Task.FromResult(Ok($"Request from client {id}"));
        }


















        // POST api/values
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
