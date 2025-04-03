namespace InforceApplicationTask.Server.Interfaces
{
    public interface IAboutRepository
    {
        Task<About> Get();
        Task Update(UpdateAboutRequest request);
    }
}
