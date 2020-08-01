using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using MicrosoftAssignment.RateLimit;

namespace MicrosoftAssignment.Attributes
{
    public class ThrottleFilter : ActionFilterAttribute
    {
        private readonly Throttler _throttler;
        public string ThrottleGroup { get; }

        public ThrottleFilter(
            int requestLimit = 1,
            int timeoutInSeconds = 3,
            [CallerMemberName] string throttleGroup = null)
        {
            ThrottleGroup = throttleGroup;
            _throttler = new Throttler(throttleGroup, requestLimit, timeoutInSeconds);
        }

        public override Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            var clientId = actionContext.ActionArguments["id"];
            _throttler.ThrottleGroup = clientId.ToString();

            if (_throttler.RequestShouldBeThrottled)
            {
                actionContext.Response = actionContext.Request.CreateResponse(
                    (HttpStatusCode)429, "Too many requests");
        
                AddThrottleHeaders(actionContext.Response);
            }
        
            return base.OnActionExecutingAsync(actionContext, cancellationToken);
        }
        
        public override Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            var clientId = actionExecutedContext.ActionContext.ActionArguments["id"];
            _throttler.ThrottleGroup = clientId.ToString();


            if (actionExecutedContext.Exception == null) _throttler.IncrementRequestCount();
            AddThrottleHeaders(actionExecutedContext.Response);
        
            return base.OnActionExecutedAsync(actionExecutedContext, cancellationToken);
        }

        private void AddThrottleHeaders(HttpResponseMessage response)
        {
            if (response == null) return;

            foreach (var header in _throttler.GetRateLimitHeaders())
            {
                response.Headers.Add(header.Key, header.Value);
            }
        }
    }
}