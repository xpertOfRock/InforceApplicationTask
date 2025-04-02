namespace InforceApplicationTask.Server.Models.Contracts
{
    public record AuthorizeRequest
    (
        string EmailOrUsername,
        string Password
    );
}
