using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCore7API.EFCore;
using NetCore7API.EFCore.Context;

namespace NetCore7API.Controllers
{
    public class DataSeedController : BaseApiController
    {
        private readonly BlogContext _context;

        public DataSeedController(BlogContext context)
        {
            _context = context;
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> SeedAsync()
        {
            await RandomDataSeeder.Seed(_context);

            return NoContent();
        }
    }
}