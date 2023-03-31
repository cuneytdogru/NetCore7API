using Microsoft.AspNetCore.Mvc;

namespace NetCore7API.Controllers
{
    [Route("/api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
    }
}