namespace InforceApplicationTask.Server.Data.Repositories
{
    public interface IShortenedUrlRepository
    {
        Task<List<ShortUrl>> GetAll();
        Task<ShortUrl?> GetById(Guid id);
        Task<bool> CheckExistingShortenedUrl(string shortenedUrl);
        Task Add(CreateShortUrlRequest request);
        Task Update(ShortUrl shortUrl);
        Task Delete(Guid id);
    }
}
