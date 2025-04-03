namespace InforceApplicationTask.Server.Models.Contracts
{
    public record UpdateAboutRequest
    (
        string UserId,
        string Description
    );
}
