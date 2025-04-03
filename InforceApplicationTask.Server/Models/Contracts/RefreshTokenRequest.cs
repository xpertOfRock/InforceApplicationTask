namespace InforceApplicationTask.Server.Models.Contracts
{
    public record RefreshTokenRequest
    (
        string Token,
        string RefreshToken
    );
}
