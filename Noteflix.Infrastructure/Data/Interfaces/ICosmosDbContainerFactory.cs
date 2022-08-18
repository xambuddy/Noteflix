namespace Noteflix.Infrastructure.Data.Interfaces
{
    public interface ICosmosDbContainerFactory
    {
        ICosmosDbContainer GetContainer(string containerName);

        Task EnsureDbSetupAsync();
    }
}
