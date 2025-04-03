using InforceApplicationTask.Server.Exceptions;
using InforceApplicationTask.Server.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InforceApplicationTask.Server.Data.Repositories
{
    public class AboutRepository(ApplicationDbContext context, ILogger<AboutRepository> logger) : IAboutRepository
    {
        public async Task<About> Get()
        {
            return await context.About.AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task Update(UpdateAboutRequest request)
        {
            await context.Database.BeginTransactionAsync();

            try
            {
                var affectedRows = await context.About
                    .ExecuteUpdateAsync(x => x
                    .SetProperty(a => a.LastUpdate, DateTime.UtcNow)
                    .SetProperty(a => a.LastUpdatedBy, request.UserId)
                    .SetProperty(a => a.Description, request.Description));

                if(affectedRows == 0)
                {
                    await context.Database.CurrentTransaction!.RollbackAsync();
                    throw new NotFoundException(typeof(ShortUrl).Name);
                }

                await context.SaveChangesAsync();

                await context.Database.CurrentTransaction!.CommitAsync();
            }
            catch (Exception e)
            {
                logger.LogError("An exception occured while updating the entity {entity}: {e}", typeof(About).Name, e.Message);
                await context.Database.CurrentTransaction!.RollbackAsync();
            }
        }
    }
}
