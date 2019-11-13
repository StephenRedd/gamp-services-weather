using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace Gamp.Weather.Api
{
    /// <summary>
    ///     Root API endpoint that outputs status and basic application information. Implements the
    ///     <see cref="Controller"/>
    /// </summary>
    /// <seealso cref="Controller"/>
    [ApiController]
    [ApiVersionNeutral]
    [Route("")]
    public class HomeController : Controller
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="env">The env.</param>
        public HomeController(IHostEnvironment env)
        {
            AppName = env.ApplicationName;
        }

        private string AppName { get; }

        /// <summary>
        ///     Gets the basic status and information about the application.
        /// </summary>
        /// <returns>ActionResult&lt;StatusInfo&gt;.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(StatusInfo), StatusCodes.Status200OK)]
        public ActionResult<StatusInfo> GetStatus()
        {
            var pathBase = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
            var pathNote = $"{pathBase}/openapi";

            return new OkObjectResult(new StatusInfo
            {
                Application = AppName,
                Status = "online",
                ApiDocumentation = pathNote
            });
        }
    }

    /// <summary>
    ///     Status Information.
    /// </summary>
    public class StatusInfo
    {
        /// <summary>
        ///     The application name.
        /// </summary>
        /// <value>The application.</value>
        public string Application { get; set; } = "Gamp.Weather";

        /// <summary>
        ///     The application status
        /// </summary>
        /// <value>The status.</value>
        public string? Status { get; set; }

        /// <summary>
        ///     Url to the API documentation.
        /// </summary>
        /// <value>The API documentation.</value>
        public string? ApiDocumentation { get; set; }
    }
}