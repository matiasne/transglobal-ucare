using GQ.Architecture.DDD.Domain.Exceptions;
using GQ.Data;
using GQ.Log;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace UCare.Web.Base
{
    /// <summary>
    /// 
    /// </summary>
    public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
    {
        /// <summary>
        /// 
        /// </summary>
        public int Order => int.MaxValue - 10;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Method intentionally left empty.
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
            {
                var exception = GQ.Core.utils.ClassUtils.GetNameObject(context.Exception.GetType());
                exception = exception.Substring(exception.LastIndexOf('.') + 1);
                dynamic data = null;

                if (context.Exception is ValidationException validationException)
                {
                    data = validationException.Details;
                }

                context.Result = new ObjectResult(new ReturnData { isError = true, data = new { Error = exception, Message = context.Exception.Message, Data = data } })
                {
                    StatusCode = 400
                };
                Log.Get().Error($"{context.ActionDescriptor.DisplayName}", context.Exception, allowStackFrame: false);
                context.ExceptionHandled = true;
            }
            else
            {
                context.Result = new ObjectResult(new ReturnData { data = ((ObjectResult)context.Result).Value });
            }
        }
    }
}
