namespace InforceApplicationTask.Server.Models.Contracts
{
    public record RegisterRequest
    (
        string Password,
        string Email,
        string Username
    );
}
