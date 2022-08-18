using Microsoft.Azure.Cosmos;

namespace Noteflix.Infrastructure.Data.Interfaces
{
    public interface ICosmosDbContainer
    {
        Container Container { get; }
    }
}
