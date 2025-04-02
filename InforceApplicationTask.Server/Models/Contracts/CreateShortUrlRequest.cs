namespace InforceApplicationTask.Server.Models.Contracts
{
    public record CreateShortUrlRequest
    (
        string UserId,
        string OriginalUrl
    );
}
