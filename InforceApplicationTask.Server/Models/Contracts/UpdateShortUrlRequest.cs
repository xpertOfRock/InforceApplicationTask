namespace InforceApplicationTask.Server.Models.Contracts
{
    public record UpdateShortUrlRequest
    (
        string Url,
        string ShortUrl
    );
}
