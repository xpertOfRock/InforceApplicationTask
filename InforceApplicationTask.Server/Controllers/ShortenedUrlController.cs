using InforceApplicationTask.Server.Data.Identity;

namespace InforceApplicationTask.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShortenedUrlController : ControllerBase
    {
        private readonly IShortenedUrlRepository _shortenedUrlRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ShortenedUrlController(
            IShortenedUrlRepository shortenedUrlRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _shortenedUrlRepository = shortenedUrlRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] ShortenedUrlRequest request)
        {
            var userId = _httpContextAccessor.HttpContext!.User.GetUserId() ?? string.Empty;

            if (string.IsNullOrEmpty(userId)) return Unauthorized("You cannot proccess this operation while being unauthorized.");

            if (!Uri.TryCreate(request.Url, UriKind.Absolute, out _))
            {
                return BadRequest("Url is invalid.");
            }           

            if(await _shortenedUrlRepository.CheckExistingLongUrl(request.Url))
            {
                return BadRequest("This url has it's shortned analog in database.");
            }

            await _shortenedUrlRepository.Add(new CreateShortUrlRequest(userId, request.Url));

            return Ok();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> Put(string id, [FromBody] UpdateShortUrlRequest request)
        {

            if (!Uri.TryCreate(request.Url, UriKind.Absolute, out _) || request.ShortUrl.Length != 10)
            {
                return BadRequest("Url or short url is invalid.");
            }

            var checkLongUrl = _shortenedUrlRepository.CheckExistingShortenedUrl(request.ShortUrl);
            var checkShortUrl = _shortenedUrlRepository.CheckExistingShortenedUrl(request.ShortUrl);

            await Task.WhenAll(checkLongUrl, checkShortUrl);

            if (checkLongUrl.Result == true && checkShortUrl.Result == true)
            {
                return BadRequest("You cannot use this original url and short url combination.");
            }

            await _shortenedUrlRepository.Update(Guid.Parse(id), new UpdateShortUrlRequest(request.Url, request.ShortUrl));

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
        [Authorize]
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

            if (shortenedUrl is null) return NotFound($"Entity {typeof(ShortUrl).Name} with Id: {id} does not exist in database.");

            if (userId != shortenedUrl.CreatedBy) return BadRequest("You cannot delete records that were not created by you.");

            await _shortenedUrlRepository.Delete(Guid.Parse(id));

            return Ok();
        }

        [HttpGet("/redirect/{shortCode}")]
        [AllowAnonymous]
        public async Task<IActionResult> RedirectToOriginalUrl(string shortCode)
        {
            var result = await _shortenedUrlRepository.GetOriginalUrlByCode(shortCode);

            if(string.IsNullOrEmpty(result))
            {
                return NotFound($"Entity {typeof(ShortUrl).Name} with ShortCode: {shortCode} does not exist in database.");
            }

            return Redirect(result);
        } 
    }
}
