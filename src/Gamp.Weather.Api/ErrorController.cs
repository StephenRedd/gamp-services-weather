using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Gamp.Weather.Api
{
    /// <summary>
    ///     Standard error controller that normalizes problem detail exception behavior between
    ///     developer and production mode behavior
    /// </summary>
    [ApiController]
    [ApiVersionNeutral]
    public class ErrorController : ControllerBase
    {
        /// <summary>
        ///     Development environment error output that includes exception details.
        /// </summary>
        /// <param name="webHostEnvironment">The hosting environment.</param>
        /// <returns>Problem details with exception details incldued.</returns>
        [Route("/error-dev")]
        public IActionResult ErrorLocalDevelopment(
            [FromServices] IWebHostEnvironment webHostEnvironment)
        {
            if (webHostEnvironment.EnvironmentName != "Development")
            {
                throw new InvalidOperationException(
                    "This shouldn't be invoked in non-development environments.");
            }

            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();

            return Problem(
                detail: context.Error.StackTrace,
                title: context.Error.Message);
        }

        /// <summary>
        ///     Standard error output that excludes exception details.
        /// </summary>
        /// <returns>Problem details with exception details excluded.</returns>
        [Route("/error")]
        public IActionResult Error() => Problem();
    }
}