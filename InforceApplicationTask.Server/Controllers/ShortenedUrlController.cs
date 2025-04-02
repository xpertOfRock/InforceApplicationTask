using InforceApplicationTask.Server.Data.Repositories;
using InforceApplicationTask.Server.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InforceApplicationTask.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShortenedUrlController : ControllerBase
    {
        private readonly IShortenedUrlRepository _shortenedUrlRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<ShortenedUrlController> _logger;
        public ShortenedUrlController(
            IShortenedUrlRepository shortenedUrlRepository,
            IHttpContextAccessor httpContextAccessor,
            ILogger<ShortenedUrlController> logger)
        {
            _shortenedUrlRepository = shortenedUrlRepository;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        [HttpPost("shorten")]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] ShortenedUrlRequest request)
        {
            var userId = _httpContextAccessor.HttpContext!.User.GetUserId() ?? string.Empty;

            if (string.IsNullOrEmpty(userId)) return Unauthorized("You cannot proccess this operation while being unauthorized.");

            if (!Uri.TryCreate(request.Url, UriKind.Absolute, out _))
            {
                return BadRequest("Url is invalid.");
            }

            await _shortenedUrlRepository.Add(new CreateShortUrlRequest(userId, request.Url));

            return Ok();
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var result = await _shortenedUrlRepository.GetAll();

            return Ok(result);
        }
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _shortenedUrlRepository.GetById(Guid.Parse(id));

            return Ok(result);
        }
        [HttpDelete("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Delete(string id)
        {
            var userId = _httpContextAccessor.HttpContext!.User.GetUserId();

            if (string.IsNullOrEmpty(userId)) return Unauthorized("You cannot proccess this operation while being unauthorized.");

            var shortenedUrl = await _shortenedUrlRepository.GetById(Guid.Parse(id));

            if (shortenedUrl is null) return BadRequest($"Entity {typeof(ShortUrl).Name} with Id: {id} does not exist in database.");

            if (userId != shortenedUrl.CreatedBy) return BadRequest("You cannot delete records that was not created by you.");

            await _shortenedUrlRepository.Delete(Guid.Parse(id));

            return Ok();
        }
    }
}
