using InforceApplicationTask.Server.Data.Identity;
using InforceApplicationTask.Server.Extensions;
using InforceApplicationTask.Server.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InforceApplicationTask.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AboutController : ControllerBase
    {
        private readonly IAboutRepository _aboutRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AboutController(
            IAboutRepository aboutRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _aboutRepository = aboutRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var aboutInfo = await _aboutRepository.Get();
            return Ok(aboutInfo);
        }

        [HttpPut]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> Update([FromBody] string description)
        {
            var userId = _httpContextAccessor.HttpContext!.User.GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("UserId is null or empty.");
            }

            var request = new UpdateAboutRequest(userId, description);

            await _aboutRepository.Update(request);

            return Ok();
        }
    }
}
