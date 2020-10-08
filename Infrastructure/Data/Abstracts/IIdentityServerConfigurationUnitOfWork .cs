using Domain.Core.Abstracts.IdentityServer;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Data.Abstracts
{
    public interface IIdentityServerConfigurationUnitOfWork
    {
        ICustomClientStore CustomClientStore { get; }
        ICustomResourceStore CustomResourceStore { get; }
        
        /// <summary>
        /// Persists changes on entities to data source
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task Commit(CancellationToken cancellationToken = default);       
    }
}
