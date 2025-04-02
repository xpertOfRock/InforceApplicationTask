using InforceApplicationTask.Server.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace InforceApplicationTask.Server.Data.Repositories
{
    public class ShortenedUrlRepository(ApplicationDbContext context, ILogger<ShortenedUrlRepository> logger) : IShortenedUrlRepository
    {
        private const int MaxSymbolsInLink = 10;
        private const string Characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private async Task<string> Generate()
        {
            while (true)
            {
                var generatedUrl = GenerateRandomString();

                if (!await context.ShortnedUrls.AnyAsync(x => x.ShortCode == generatedUrl))
                {
                    return generatedUrl;
                }
            }
        }
        private string GenerateRandomString()
        {
            Random random = new();

            var resultChars = new char[MaxSymbolsInLink];

            for (int i = 0; i < MaxSymbolsInLink; i++)
            {
                resultChars[i] = Characters[random.Next(Characters.Length - 1)];
            }

            return new string(resultChars);
        }
        public async Task Add(CreateShortUrlRequest request)
        {
            await context.Database.BeginTransactionAsync();

            try
            {

                var code = await Generate();

                var shortUrl = new ShortUrl
                {
                    Id = Guid.NewGuid(),
                    CreatedBy = request.UserId,
                    ShortCode = code,
                    OriginalUrl = request.OriginalUrl,
                    CreatedDate = DateTime.UtcNow
                };

                await context.ShortnedUrls.AddAsync(shortUrl);

                await context.SaveChangesAsync();

                await context.Database.CurrentTransaction!.CommitAsync();
            }
            catch(Exception e)
            {
                logger.LogError("An exception occured while adding new record of the entity {entity}: {e}", typeof(ShortUrl).Name, e.Message);
                await context.Database.CurrentTransaction!.RollbackAsync();
            }
        }

        public async Task Delete(Guid id)
        {
            await context.Database.BeginTransactionAsync();

            try
            {
                var affectedRows = await context.ShortnedUrls
                    .Where(x => x.Id == id)
                    .ExecuteDeleteAsync();

                if (affectedRows == 0)
                {
                    await context.Database.CurrentTransaction!.RollbackAsync();
                    throw new NotFoundException<ShortUrl>();
                }

                await context.SaveChangesAsync();

                await context.Database.CurrentTransaction!.CommitAsync();
            }
            catch(Exception e)
            {
                logger.LogError("An exception occured while deleting the entity {entity}: {e}", typeof(ShortUrl).Name, e.Message);
                await context.Database.CurrentTransaction!.RollbackAsync();
            }           
        }

        public async Task<List<ShortUrl>> GetAll()
        {
            return await context.ShortnedUrls
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ShortUrl?> GetById(Guid id)
        {
            return await context.ShortnedUrls
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> CheckExistingShortenedUrl(string shortenedUrl)
        {
            return await context.ShortnedUrls.AnyAsync(x => x.ShortCode == shortenedUrl);
        }

        public async Task Update(ShortUrl shortUrl)
        {
            await context.Database.BeginTransactionAsync();

            try
            {
                var affectedRows = await context.ShortnedUrls
                .Where(x => x.Id == shortUrl.Id)
                .ExecuteUpdateAsync(x => x
                .SetProperty(su => su.OriginalUrl, shortUrl.OriginalUrl)
                .SetProperty(su => su.ShortCode, shortUrl.ShortCode));

                if (affectedRows == 0)
                {
                    await context.Database.CurrentTransaction!.RollbackAsync();
                    throw new NotFoundException<ShortUrl>();
                }

                await context.SaveChangesAsync();

                await context.Database.CurrentTransaction!.CommitAsync();
            }
            catch(Exception e)
            {
                logger.LogError("An exception occured while updating the entity {entity}: {e}", typeof(ShortUrl).Name ,e.Message);
                await context.Database.CurrentTransaction!.RollbackAsync();
            }                      
        }
    }
}
