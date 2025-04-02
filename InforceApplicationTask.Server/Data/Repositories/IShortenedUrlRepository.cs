namespace InforceApplicationTask.Server.Data.Repositories
{
    public interface IShortenedUrlRepository
    {
        Task<List<ShortUrl>> GetAll();
        Task<ShortUrl?> GetById(Guid id);
        Task<bool> CheckExistingShortenedUrl(string shortenedUrl);
        Task<bool> CheckExistingLongUrl(string url);
        Task Add(CreateShortUrlRequest request);
        Task Update(Guid id, UpdateShortUrlRequest request);
        Task Delete(Guid id);
        Task<ShortUrl?> GetByCode(string code);
    }
}
